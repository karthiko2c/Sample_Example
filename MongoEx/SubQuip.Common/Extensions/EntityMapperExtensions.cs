using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Omu.ValueInjecter.Injections;
using System;
using MongoDB.Bson;

namespace SubQuip.Common.Extensions
{
    /// <summary>
    /// Extension Methods for Entity Mapper.
    /// </summary>
    public static class EntityMapperExtensions
    {
        public static List<TViewModel> MapFromModel<TModel, TViewModel>(this object target, List<TModel> model, string ignoreProperties = null)
            where TModel : class, new()
            where TViewModel : class, new()
        {
            return model.ToList().Select(o => new TViewModel().
                                                    InjectFrom(new IgnoreProperty(ignoreProperties), o).
                                                    InjectFrom<MapFromBsonObjectId>(o)).Cast<TViewModel>().ToList();
        }

        public static object MapFromModel<TModel>(this object target, TModel model, string ignoreProperties = null) where TModel : class, new()
        {
            target.InjectFrom(new IgnoreProperty(ignoreProperties), model).InjectFrom<MapFromBsonObjectId>(model);
            return target;
        }

        public static object MapFromViewModel<TModel>(this object target, TModel viewModel, ClaimsIdentity identity = null, string ignoreProperties = null) where TModel : class, new()
        {
            target.InjectFrom(new IgnoreProperty(ignoreProperties), viewModel)
                 .InjectFrom<AvoidNullProps>(viewModel).InjectFrom<MapToBsonObjectId>(viewModel);
            if (identity != null)
            {
                target.MapAuditColumns(identity);
            }
            return target;
        }
    }

    public class MapFromBsonObjectId : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return source.GetTypeInfo().FullName == "MongoDB.Bson.ObjectId" && target.GetTypeInfo().FullName == "System.String";
        }

        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);
            if (val != null)
            {
                if (sp.PropertyType.FullName == "MongoDB.Bson.ObjectId")
                {
                    tp.SetValue(target, val.ToString());
                }
                else
                {
                    tp.SetValue(target, val);
                }
            }

        }
    }

    public class MapToBsonObjectId : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return target.GetTypeInfo().FullName == "MongoDB.Bson.ObjectId" && source.GetTypeInfo().FullName == "System.String";
        }

        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);
            if (val != null)
            {
                if (tp.PropertyType.FullName == "MongoDB.Bson.ObjectId")
                {
                    tp.SetValue(target, ObjectId.Parse(val.ToString()));
                }
                else
                {
                    tp.SetValue(target, val);
                }
            }

        }
    }



    public class AvoidNullProps : LoopInjection
    {
        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);
            if (val != null)
                tp.SetValue(target, val);
        }
    }

    public class IgnoreProperty : LoopInjection
    {
        private readonly string[] _ignoreProperties;

        public IgnoreProperty(string ignoreProperties)
        {
            if (ignoreProperties != null) { _ignoreProperties = ignoreProperties.Split(';'); }
        }
    }
}
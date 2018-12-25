using RS.Common.Enums;

namespace RS.ViewModel.Skill
{
    public class SkillViewModel
    {
        public int SkillId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OpeningSkillType OpeningSkillType { get; set; }
    }
}

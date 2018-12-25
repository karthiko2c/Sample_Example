import { SeriesModel } from './series-model';

export class ChartModel {
    series?: SeriesModel[];
    type?: number;
    chartType?: string;
    xCategories?: string[];
}

import { OpeningSkillType } from '../../app.enum';

export class SkillModel {
    skillId?: number;
    name?: string;
    description?: string;
    openingSkillType?: OpeningSkillType;
    isChecked?: boolean;
}

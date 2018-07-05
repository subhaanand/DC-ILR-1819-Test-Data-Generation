﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using DCT.ILR.Model;

namespace DCT.TestDataGenerator.Functor
{
    public class LearnDelFAMDateTo_04
        : ILearnerMultiMutator
    {
        private ILearnerCreatorDataCache _dataCache;
        private GenerationOptions _options;

        public FilePreparationDateRequired FilePreparationDate()
        {
            return FilePreparationDateRequired.July;
        }

        public string RuleName()
        {
            return "LearnDelFAMDateTo_04";
        }

        public string LearnerReferenceNumberStub()
        {
            return "Ldfamto04";
        }

        public IEnumerable<LearnerTypeMutator> LearnerMutators(ILearnerCreatorDataCache cache)
        {
            return new List<LearnerTypeMutator>()
            {
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Adult, DoMutateLearner = MutateLSF, DoMutateOptions = MutateGenerationOptions, ExclusionRecord = true },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.NonFunded, DoMutateLearner = MutateALB, DoMutateOptions = MutateGenerationOptionsALB, ExclusionRecord = true },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = MutateACT, DoMutateOptions = MutateGenerationOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = MutateLDM, DoMutateOptions = MutateGenerationOptions, ExclusionRecord = true }
            };
        }

        private void MutateACT(MessageLearner learner, bool valid)
        {
            var lds = learner.LearningDelivery.ToList();
            learner.DateOfBirth = learner.LearningDelivery[0].LearnStartDate.AddYears(-19).AddMonths(-3);

            if (!valid)
            {
                foreach (var ld in lds)
                {
                    foreach (MessageLearnerLearningDelivery mld in lds)
                    {
                        {
                            var ldfams = ld.LearningDeliveryFAM.Where(s => s.LearnDelFAMType != LearnDelFAMType.ACT.ToString());
                            ld.LearningDeliveryFAM = ldfams.ToArray();
                        }

                        var lfams = ld.LearningDeliveryFAM.ToList();
                        lfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                        {
                            LearnDelFAMType = LearnDelFAMType.ACT.ToString(),
                            LearnDelFAMCode = ((int)LearnDelFAMCode.ACT_ContractEmployer).ToString(),
                            LearnDelFAMDateToSpecified = false
                        });
                        ld.LearningDeliveryFAM = lfams.ToArray();
                        break;
                    }
                }
            }
        }

        private void MutateLSF(MessageLearner learner, bool valid)
        {
            var ld = learner.LearningDelivery[0];
            var ldfams = ld.LearningDeliveryFAM.ToList();
            learner.DateOfBirth = learner.LearningDelivery[0].LearnStartDate.AddYears(-19).AddMonths(-3);
            if (valid)
            {
                ldfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.LSF.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.LSF).ToString(),
                    LearnDelFAMDateFrom = ld.LearnStartDate,
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateTo = ld.LearnPlanEndDate,
                    LearnDelFAMDateToSpecified = true
                });
            }
            else if (!valid)
            {
                ldfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.LSF.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.LSF).ToString(),
                });
            }

            ld.LearningDeliveryFAM = ldfams.ToArray();
        }

        private void MutateALB(MessageLearner learner, bool valid)
        {
            var ld = learner.LearningDelivery[0];
            var ldfams = ld.LearningDeliveryFAM.ToList();
            ld.LearnAimRef = "6030599X";
            learner.DateOfBirth = learner.LearningDelivery[0].LearnStartDate.AddYears(-19).AddMonths(-3);
            if (valid)
            {
                ldfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.ALB.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.ALB_Rate_1).ToString(),
                    LearnDelFAMDateFrom = ld.LearnStartDate,
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateTo = ld.LearnPlanEndDate,
                    LearnDelFAMDateToSpecified = true
                });
            }
            else if (!valid)
            {
                ldfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.ALB.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.ALB_Rate_1).ToString(),
                });
            }

            ld.LearningDeliveryFAM = ldfams.ToArray();
        }

        private void MutateLDM(MessageLearner learner, bool valid)
        {
            var ld = learner.LearningDelivery[0];
            var ldfams = ld.LearningDeliveryFAM.ToList();
            learner.DateOfBirth = learner.LearningDelivery[0].LearnStartDate.AddYears(-19).AddMonths(-3);
            if (valid)
            {
                ldfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.LDM.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.LDM_ApprenticeshipTrainingAgency).ToString()
                });
            }
            else if (!valid)
            {
                ldfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.LDM.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.LDM_ApprenticeshipTrainingAgency).ToString(),
                    LearnDelFAMDateFrom = ld.LearnStartDate,
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateTo = ld.LearnPlanEndDate,
                    LearnDelFAMDateToSpecified = true
                });
            }

            ld.LearningDeliveryFAM = ldfams.ToArray();
        }

        private void MutateGenerationOptions(GenerationOptions options)
        {
        }

        private void MutateGenerationOptionsALB(GenerationOptions options)
        {
            _options = options;
            _options.EmploymentRequired = true;
            _options.LD.IncludeADL = true;
        }
    }
}

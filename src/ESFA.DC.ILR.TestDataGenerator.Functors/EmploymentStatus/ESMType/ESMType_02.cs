﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using DCT.ILR.Model;

namespace DCT.TestDataGenerator.Functor
{
    public class ESMType_02
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
            return "ESMType_02";
        }

        public string LearnerReferenceNumberStub()
        {
            return "ESMType02";
        }

        public IEnumerable<LearnerTypeMutator> LearnerMutators(ILearnerCreatorDataCache cache)
        {
            _dataCache = cache;
            return new List<LearnerTypeMutator>()
            {
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Adult, DoMutateLearner = MutateLES, DoMutateOptions = MutateGenerationOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.OtherAdult, DoMutateLearner = MutateLES, DoMutateOptions = MutateGenerationOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.NonFunded, DoMutateLearner = MutateLES, DoMutateOptions = MutateGenerationOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = MutateLES, DoMutateOptions = MutateGenerationOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.CommunityLearning, DoMutateLearner = MutateCommunity, DoMutateOptions = MutateGenerationOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.ESF, DoMutateLearner = MutateLES, DoMutateOptions = MutateGenerationOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Adult, DoMutateLearner = MutateLDMType, DoMutateOptions = MutateGenerationOptions, ExclusionRecord = true }
            };
        }

        private void MutateLES(MessageLearner learner, bool valid)
        {
            learner.DateOfBirth = learner.LearningDelivery[0].LearnStartDate.AddYears(-19).AddMonths(-3);
            if (valid)
            {
                var les = learner.LearnerEmploymentStatus[0];
                les.EmpStatSpecified = true;
                les.EmpStat = 10;
                les.DateEmpStatAppSpecified = true;
                les.DateEmpStatApp = learner.LearningDelivery[0].LearnStartDate.AddDays(-2);
            }

            if (!valid)
            {
                var les = learner.LearnerEmploymentStatus[0];
                var lesm = les.EmploymentStatusMonitoring.ToList();
                lesm.RemoveRange(0, 2);
                lesm.Add(new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring()
                {
                    ESMType = EmploymentStatusMonitoringType.LOE.ToString(),
                    ESMCode = (int)EmploymentStatusMonitoringCode.Employed712,
                    ESMCodeSpecified = true
                });

                learner.LearnerEmploymentStatus[0].EmploymentStatusMonitoring = lesm.ToArray();
            }
        }

        private void MutateLDMType(MessageLearner learner, bool valid)
        {
            if (valid)
            {
                MutateLES(learner, valid);
            }

            if (!valid)
            {
                    var les = learner.LearnerEmploymentStatus[0];
                    var lesm = les.EmploymentStatusMonitoring.ToList();
                    lesm.RemoveRange(0, 2);
                    les.EmpStatSpecified = true;
                    les.EmpStat = 11;
                    les.DateEmpStatAppSpecified = true;
                    les.DateEmpStatApp = new DateTime(2012, 09, 30);
            }
        }

        private void MutateCommunity(MessageLearner learner, bool valid)
        {
            if (valid)
            {
                var led = learner.LearningDelivery[0];
                var ldfams = led.LearningDeliveryFAM.ToList();
                ldfams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.SOF.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.SOF_ESFA_Adult).ToString(),
                });
                led.LearningDeliveryFAM = ldfams.ToArray();
            }

            if (!valid)
            {
                var les = learner.LearnerEmploymentStatus[0];
                var lesm = les.EmploymentStatusMonitoring.ToList();
                lesm.RemoveRange(0, 2);
                lesm.Add(new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring()
                {
                    ESMType = EmploymentStatusMonitoringType.LOE.ToString(),
                    ESMCode = (int)EmploymentStatusMonitoringCode.Employed712,
                    ESMCodeSpecified = true
                });

                learner.LearnerEmploymentStatus[0].EmploymentStatusMonitoring = lesm.ToArray();
            }
        }

        private void MutateTrainee(MessageLearner learner, bool valid)
        {
            if (!valid)
            {
                learner.LearningDelivery[0].ProgType = 24;
                learner.LearningDelivery[0].ProgTypeSpecified = true;
                var les = learner.LearnerEmploymentStatus[0];
                les.DateEmpStatAppSpecified = true;
                les.DateEmpStatApp = learner.LearningDelivery[0].LearnStartDate.AddDays(+2);
            }
        }

        private void MutateGenerationOptions(GenerationOptions options)
        {
            options.EmploymentRequired = true;
        }
    }
}
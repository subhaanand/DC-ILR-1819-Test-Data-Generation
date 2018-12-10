﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using DCT.ILR.Model;

namespace DCT.TestDataGenerator.Functor
{
    public class R20
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
            return "R20";
        }

        public string LearnerReferenceNumberStub()
        {
            return "R20";
        }

        public IEnumerable<LearnerTypeMutator> LearnerMutators(ILearnerCreatorDataCache cache)
        {
            return new List<LearnerTypeMutator>()
            {
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate, DoMutateOptions = MutateOptions },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = MutateApprenticeshipStandard, DoMutateOptions = MutateOptions, ExclusionRecord = true },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = MutateProgType, DoMutateOptions = MutateGenerationOptions, ExclusionRecord = true },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = MutateAimType, DoMutateOptions = MutateGenerationOptions, ExclusionRecord = true }
            };
        }

        private void Mutate(MessageLearner learner, bool valid)
        {
            learner.DateOfBirth = learner.LearningDelivery[0].LearnStartDate.AddYears(-19).AddMonths(-3);
            if (!valid)
            {
                learner.LearningDelivery[0].LearnAimRef = "60005105";
                learner.LearningDelivery[0].AimTypeSpecified = true;
                learner.LearningDelivery[0].AimType = 3;
                learner.LearningDelivery[0].FworkCodeSpecified = true;
                learner.LearningDelivery[0].FworkCode = 403;
                learner.LearningDelivery[0].LearnActEndDateSpecified = true;
                learner.LearningDelivery[0].LearnActEndDate = new DateTime(2017, 11, 30);
            }
        }

        private void MutateApprenticeshipStandard(MessageLearner learner, bool valid)
        {
            Mutate(learner, valid);
            if (!valid)
            {
                learner.LearningDelivery[0].ProgTypeSpecified = true;
                learner.LearningDelivery[0].ProgType = (int)ProgType.ApprenticeshipStandard;
            }
        }

        private void MutateProgType(MessageLearner learner, bool valid)
        {
            Mutate(learner, valid);
            if (!valid)
            {
                learner.LearningDelivery[0].ProgTypeSpecified = false;
            }
        }

        private void MutateAimType(MessageLearner learner, bool valid)
        {
            Mutate(learner, valid);
            if (!valid)
            {
                learner.LearningDelivery[1].LearnAimRef = "60005105";
                learner.LearningDelivery[1].AimTypeSpecified = true;
                learner.LearningDelivery[1].AimType = 3;
                learner.LearningDelivery[1].FworkCodeSpecified = true;
                learner.LearningDelivery[1].FworkCode = 403;
                learner.LearningDelivery[1].LearnStartDate = new DateTime(2017, 11, 30).AddDays(1);
            }
        }

        private void MutateGenerationOptions(GenerationOptions options)
        {
            options.EmploymentRequired = true;
        }

        private void MutateOptions(GenerationOptions options)
        {
        }
    }
}

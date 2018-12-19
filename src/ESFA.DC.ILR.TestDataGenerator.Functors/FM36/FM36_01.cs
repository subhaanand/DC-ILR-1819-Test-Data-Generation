﻿using System;
using System.Collections.Generic;
using System.Linq;
using DCT.ILR.Model;

namespace DCT.TestDataGenerator.Functor
{
    /// <summary>
    /// The FM35 test data generator helps create a range of high quality test data specifically to test the FM35 funding model
    /// It starts with simple learners but becomes more and more complex in what the earning calculation has to do and areas where in the past there have been specific
    /// issues and bugs
    /// </summary>
    public class FM36_01
        : ILearnerMultiMutator
    {
        private ILearnerCreatorDataCache _dataCache;
        private GenerationOptions _options;
        private DateTime _outcomeDate;

        public FilePreparationDateRequired FilePreparationDate()
        {
            return FilePreparationDateRequired.July;
        }

        public IEnumerable<LearnerTypeMutator> LearnerMutators(ILearnerCreatorDataCache cache)
        {
            _dataCache = cache;
            return new List<LearnerTypeMutator>()
            {
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate19Apprenticeship, DoMutateOptions = MutateGenerationOptionsOlderApprenticeship },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate19ApprenticeshipLowercaseAim, DoMutateOptions = MutateGenerationOptionsOlderApprenticeship },
//                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate16ApprenticeshipCoFunded, DoMutateOptions = MutateGenerationOptionsOlderApprenticeship },
//                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate16ApprenticeshipCoFundedLDPostcodeAreaCost, DoMutateOptions = MutateGenerationOptionsOlderApprenticeship },
//                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate19ApprenticeshipCoFundedLDPostcodeAreaCost, DoMutateOptions = MutateGenerationOptionsOlderApprenticeship },
//                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate19ApprenticeshipCoFundedLDPostcodeAreaCostLDMATA, DoMutateOptions = MutateGenerationOptionsOlderApprenticeship },
//                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate16ApprenticeshipSimpleRestart, DoMutateOptions = MutateGenerationOptionsOlderApprenticeshipLD2 },
////4)	Restarts.
////a.  Simple model
////i.  A-S-O….comp=6 with end date
////ii. J-F restart, prior learning %
//                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Adult, DoMutateLearner = Mutate19LD2Restarts, DoMutateOptions = MutateGenerationOptionsLD2, DoMutateProgression = Mutate19LD2RestartsDestAndProg },
////b.  Complex apprenticeship model
////i.  Break in ZPROG01
////ii. Two components, first finishes before end of zprog aim (so no achievement payment)
////iii.    The zprog + second then complete. The achievement payment for the 1st component should then appear when the zprog is achieved
//                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate16ApprenticeshipComplexRestart, DoMutateOptions = MutateGenerationOptionsOlderApprenticeshipLD3, DoMutateProgression = Mutate16ApprenticeshipComplexRestartsDestAndProg },
                new LearnerTypeMutator() { LearnerType = LearnerTypeRequired.Apprenticeships, DoMutateLearner = Mutate19ApprenticeshipMultiYearComplexRestartWithFM35, DoMutateOptions = MutateGenerationOptionsOlderApprenticeshipLD10, DoMutateProgression = MutateOlderApprenticeshipComplexRestartsDestAndProg },
            };
        }

        public string RuleName()
        {
            return "FM36_01";
        }

        public string LearnerReferenceNumberStub()
        {
            return "fm36_01";
        }

        private void Mutate19(MessageLearner learner, bool valid)
        {
            Helpers.MutateDOB(learner, valid, Helpers.AgeRequired.Exact19, Helpers.BasedOn.LearnDelStart, Helpers.MakeOlderOrYoungerWhenInvalid.NoChange);
            MutateCommon(learner, valid);
        }

        private void Mutate19LDPostcodeAreaCost(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
            learner.LearningDelivery[0].DelLocPostCode = _dataCache.PostcodeWithAreaCostFactor();
        }

        private void Mutate19DisadvantagedPostcodeRate(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
            learner.PostcodePrior = _dataCache.PostcodeDisadvantagedArea();
        }

        private void Mutate19DisadvantagedPostcodeRateWithAreaCost(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
            learner.LearningDelivery[0].DelLocPostCode = _dataCache.PostcodeWithAreaCostFactor();
            learner.PostcodePrior = _dataCache.PostcodeDisadvantagedArea();
        }

        private void MutateCommon(MessageLearner learner, bool valid)
        {
        }

        private void Mutate19FFI(MessageLearner learner, bool valid)
        {
            MutateCommon(learner, valid);
            Helpers.RemoveLearningDeliveryFAM(learner, LearnDelFAMType.FFI);
            foreach (MessageLearnerLearningDelivery ld in learner.LearningDelivery)
            {
                var ld0Fams = ld.LearningDeliveryFAM.ToList();
                ld0Fams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
                {
                    LearnDelFAMType = LearnDelFAMType.FFI.ToString(),
                    LearnDelFAMCode = ((int)LearnDelFAMCode.FFI_Fully).ToString()
                });
                ld.LearningDeliveryFAM = ld0Fams.ToArray();
            }
        }

        private void Mutate19LD3(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
            learner.LearningDelivery[1].LearnAimRef = "60126334";
        }

        private void Mutate19Apprenticeship(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
//            Helpers.MutateApprenticeshipToOlderWithFundingFlag(learner, LearnDelFAMCode.FFI_Fully);
        }

        private void Mutate19ApprenticeshipLowercaseAim(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
            learner.LearningDelivery[1].LearnAimRef = "6008862x";
        }

        private void Mutate16ApprenticeshipCoFunded(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
//            Helpers.MutateApprenticeshipToOlderWithFundingFlag(learner, LearnDelFAMCode.FFI_Co);
        }

        private void Mutate16ApprenticeshipCoFundedLDPostcodeAreaCost(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
//            Helpers.MutateApprenticeshipToOlderWithFundingFlag(learner, LearnDelFAMCode.FFI_Co);
            learner.LearningDelivery[1].DelLocPostCode = _dataCache.PostcodeWithAreaCostFactor();
        }

        private void Mutate19ApprenticeshipCoFundedLDPostcodeAreaCost(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
//            Helpers.MutateApprenticeshipToOlderWithFundingFlag(learner, LearnDelFAMCode.FFI_Co);
            Helpers.MutateDOB(learner, valid, Helpers.AgeRequired.Exact19, Helpers.BasedOn.LearnDelStart, Helpers.MakeOlderOrYoungerWhenInvalid.NoChange);
            learner.LearningDelivery[1].DelLocPostCode = _dataCache.PostcodeWithAreaCostFactor();
        }

        private void Mutate19ApprenticeshipCoFundedLDPostcodeAreaCostLDMATA(MessageLearner learner, bool valid)
        {
            Mutate19ApprenticeshipCoFundedLDPostcodeAreaCost(learner, valid);

            var ld1Fams = learner.LearningDelivery[1].LearningDeliveryFAM.ToList();

            // create lots of LDM based LD FAMS
            ld1Fams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
            {
                LearnDelFAMType = LearnDelFAMType.LDM.ToString(),
                LearnDelFAMCode = ((int)LearnDelFAMCode.LDM_PrincesTrustTeamProgramme).ToString()
            });
            ld1Fams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
            {
                LearnDelFAMType = LearnDelFAMType.LDM.ToString(),
                LearnDelFAMCode = ((int)LearnDelFAMCode.LDM_SteelRedundancy).ToString()
            });
            ld1Fams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
            {
                LearnDelFAMType = LearnDelFAMType.LDM.ToString(),
                LearnDelFAMCode = ((int)LearnDelFAMCode.LDM_HESA_GeneratedILRfile).ToString()
            });
            ld1Fams.Add(new MessageLearnerLearningDeliveryLearningDeliveryFAM()
            {
                LearnDelFAMType = LearnDelFAMType.LDM.ToString(),
                LearnDelFAMCode = ((int)LearnDelFAMCode.LDM_ApprenticeshipTrainingAgency).ToString()
            });
            learner.LearningDelivery[1].LearningDeliveryFAM = ld1Fams.ToArray();
        }

        private void Mutate16ApprenticeshipSimpleRestart(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
//            Helpers.MutateApprenticeshipToOlderWithFundingFlag(learner, LearnDelFAMCode.FFI_Co);
            var lds = learner.LearningDelivery.ToList();
            lds[0].LearnPlanEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(365);
            lds[1].LearnActEndDate = lds[1].LearnStartDate + TimeSpan.FromDays(45);
            lds[1].LearnPlanEndDate = lds[0].LearnPlanEndDate;
            lds[1].LearnActEndDateSpecified = true;

            lds[1].CompStatus = (int)CompStatus.BreakInLearning;
            lds[1].Outcome = (int)Outcome.NoAchievement;
            lds[1].OutcomeSpecified = true;

            lds[2].LearnStartDate = lds[1].LearnActEndDate + TimeSpan.FromDays(30);
            Helpers.AddLearningDeliveryRestartFAM(lds[2]);
            lds[2].PriorLearnFundAdj = 80;
            lds[2].PriorLearnFundAdjSpecified = true;
            lds[2].LearnPlanEndDate = lds[0].LearnPlanEndDate;
            //            Helpers.SetLearningDeliveryEndDates(lds[2], lds[0].LearnPlanEndDate, Helpers.SetAchDate.DoNotSetAchDate);
        }

        //b.  Complex apprenticeship model
        //i.  Break in ZPROG01
        //ii. Two components, first finishes before end of zprog aim (so no achievement payment)
        //iii.    The zprog + second then complete. The achievement payment for the 1st component should then appear when the zprog is achieved
        private void Mutate16ApprenticeshipComplexRestart(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
//            Helpers.MutateApprenticeshipToOlderWithFundingFlag(learner, LearnDelFAMCode.FFI_Co);
            Helpers.MutateDOB(learner, valid, Helpers.AgeRequired.Exact19, Helpers.BasedOn.LearnDelStart, Helpers.MakeOlderOrYoungerWhenInvalid.NoChange);

            //the data starts as one zprog and 4 components aims.
            var lds = learner.LearningDelivery.ToList();
            // reset date as the muteate will have side effects
            lds[0].LearnStartDate = _options.LD.OverrideLearnStartDate.Value;
            lds[0].LearnActEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(180);
            lds[0].LearnPlanEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(365);
            lds[0].LearnActEndDateSpecified = true;

            lds[0].CompStatus = (int)CompStatus.BreakInLearning;
            lds[0].Outcome = (int)Outcome.NoAchievement;
            lds[0].OutcomeSpecified = true;

            lds[1].LearnAimRef = lds[0].LearnAimRef;
            lds[1].LearnStartDate = lds[0].LearnActEndDate + TimeSpan.FromDays(30);
            lds[1].AimType = lds[0].AimType;
            Helpers.AddLearningDeliveryRestartFAM(lds[1]);
            lds[1].PriorLearnFundAdj = 25;
            lds[1].PriorLearnFundAdjSpecified = true;
            lds[1].LearnPlanEndDate = lds[0].LearnPlanEndDate;
            lds[1].OrigLearnStartDate = lds[0].LearnStartDate;
            lds[1].OrigLearnStartDateSpecified = true;
            Helpers.SetLearningDeliveryEndDates(lds[1], lds[1].LearnPlanEndDate, Helpers.SetAchDate.DoNotSetAchDate);

            lds[2].LearnActEndDate = lds[0].LearnActEndDate + TimeSpan.FromDays(-1);
            lds[2].LearnActEndDateSpecified = true;
            lds[2].LearnPlanEndDate = lds[2].LearnActEndDate;
            lds[2].CompStatus = (int)CompStatus.Completed;
            lds[2].Outcome = (int)Outcome.Achieved;
            lds[2].OutcomeSpecified = true;

            lds[3].LearnAimRef = _dataCache.ApprenticeshipAims((ProgType)lds[0].ProgType).
                Where(s => s.PwayCode == lds[0].PwayCode && s.FworkCode == lds[0].FworkCode && s.LearningDelivery.LearnAimRef != lds[2].LearnAimRef)
                .First()
                .LearningDelivery.LearnAimRef;
            lds[3].LearnStartDate = lds[1].LearnStartDate;
            lds[3].LearnPlanEndDate = lds[1].LearnActEndDate;
            lds[3].LearnActEndDate = lds[1].LearnPlanEndDate;
            lds[3].LearnActEndDateSpecified = true;
            lds[3].CompStatus = (int)CompStatus.Completed;
            lds[3].Outcome = (int)Outcome.Achieved;
            lds[3].OutcomeSpecified = true;

            _outcomeDate = lds[3].LearnActEndDate;
        }

        private void Mutate19ApprenticeshipMultiYearComplexRestartWithFM35(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
//            Helpers.MutateApprenticeshipToOlderWithFundingFlag(learner, LearnDelFAMCode.FFI_Co);
            Helpers.MutateDOB(learner, valid, Helpers.AgeRequired.Exact19, Helpers.BasedOn.LearnDelStart, Helpers.MakeOlderOrYoungerWhenInvalid.NoChange);
            learner.DateOfBirth = DateTime.Parse("1969-MAR-27");
            //the data starts as one zprog and components aims.
            var lds = learner.LearningDelivery.ToList();
            //reset date as the mutate will have side effects
            lds[0].LearnStartDate = _options.LD.OverrideLearnStartDate.Value;
            lds[0].LearnActEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(125);
            lds[0].LearnPlanEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(495);
            lds[0].LearnActEndDateSpecified = true;
            lds[0].CompStatus = (int)CompStatus.BreakInLearning;
            lds[0].Outcome = (int)Outcome.NoAchievement;
            lds[0].OutcomeSpecified = true;
            lds[0].ProgType = 2;
            lds[0].FworkCode = 490;
            lds[0].PwayCode = 1;
            lds[0].DelLocPostCode = "KT16 0PZ";

            lds[1].LearnStartDate = lds[0].LearnStartDate + TimeSpan.FromDays(391);
            lds[1].OrigLearnStartDate = lds[0].LearnStartDate;
            lds[1].OrigLearnStartDateSpecified = true;
            lds[1].LearnPlanEndDate = lds[1].LearnStartDate + TimeSpan.FromDays(370);
            lds[1].CompStatus = (int)CompStatus.Continuing;
            lds[1].ProgType = lds[0].ProgType;
            lds[1].FworkCode = lds[0].FworkCode;
            lds[1].PwayCode = lds[0].PwayCode;
            lds[1].DelLocPostCode = "KT16 0PZ";
            lds[1].LearnAimRef = lds[0].LearnAimRef;
            lds[1].AimType = lds[0].AimType;
            lds[1].OutcomeSpecified = false;
            Helpers.AddLearningDeliveryRestartFAM(lds[1]);

            for (int i = 2; i < 10; i += 2)
            {
                lds[i].LearnAimRef = _dataCache.ApprenticeshipAims((ProgType)lds[0].ProgType, lds[0].FworkCode, lds[0].PwayCode, (i / 2) - 1).LearningDelivery.LearnAimRef;
                lds[i].ProgType = lds[0].ProgType;
                lds[i].FworkCode = lds[0].FworkCode;
                lds[i].PwayCode = lds[0].PwayCode;
                lds[i].LearnStartDate = lds[0].LearnActEndDate + TimeSpan.FromDays(30);
                lds[i].AimType = (int)AimType.ComponentAim;
                lds[i].LearnStartDate = lds[0].LearnStartDate;
                lds[i].LearnPlanEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(221);
                lds[i].LearnActEndDate = lds[0].LearnActEndDate;
                lds[i].LearnActEndDateSpecified = lds[0].LearnActEndDateSpecified;
                lds[i].CompStatus = lds[0].CompStatus;
                lds[i].CompStatusSpecified = lds[0].CompStatusSpecified;
                lds[i].Outcome = lds[0].Outcome;
                lds[i].OutcomeSpecified = lds[0].OutcomeSpecified;
                lds[i].DelLocPostCode = "KT16 0PZ";
            }

            for (int i = 3; i < 10; i += 2)
            {
                lds[i].LearnAimRef = _dataCache.ApprenticeshipAims((ProgType)lds[0].ProgType, lds[0].FworkCode, lds[0].PwayCode, ((i - 1) / 2) - 1).LearningDelivery.LearnAimRef;
                lds[i].ProgType = lds[0].ProgType;
                lds[i].FworkCode = lds[0].FworkCode;
                lds[i].PwayCode = lds[0].PwayCode;
                lds[i].AimType = (int)AimType.ComponentAim;
                lds[i].LearnStartDate = lds[1].LearnStartDate;
                lds[i].LearnPlanEndDate = lds[1].LearnStartDate + TimeSpan.FromDays(96);
                lds[i].CompStatus = lds[1].CompStatus;
                lds[i].CompStatusSpecified = lds[1].CompStatusSpecified;
                lds[i].Outcome = lds[1].Outcome;
                lds[i].OutcomeSpecified = lds[1].OutcomeSpecified;
                lds[i].DelLocPostCode = "KT16 0PZ";
                lds[i].PriorLearnFundAdj = 38;
                lds[i].PriorLearnFundAdjSpecified = true;
                Helpers.AddLearningDeliveryRestartFAM(lds[i]);
            }

            lds[9].LearnPlanEndDate = lds[9].LearnStartDate + TimeSpan.FromDays(370);
            lds[9].PriorLearnFundAdj = 72;

            _outcomeDate = lds[0].LearnActEndDate;
        }

        private void Mutate19LD2Restarts(MessageLearner learner, bool valid)
        {
            Mutate19(learner, valid);
            var lds = learner.LearningDelivery.ToList();
            lds[0].LearnActEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(45);
            lds[0].LearnPlanEndDate = lds[0].LearnStartDate + TimeSpan.FromDays(75);
            lds[0].LearnActEndDateSpecified = true;

            lds[0].CompStatus = (int)CompStatus.BreakInLearning;
            lds[0].Outcome = (int)Outcome.NoAchievement;
            lds[0].OutcomeSpecified = true;

            lds[1].LearnStartDate = lds[0].LearnActEndDate + TimeSpan.FromDays(30);
            Helpers.AddLearningDeliveryRestartFAM(lds[1]);
            lds[1].PriorLearnFundAdj = 50;
            lds[1].PriorLearnFundAdjSpecified = true;
            lds[1].LearnPlanEndDate = lds[0].LearnPlanEndDate + TimeSpan.FromDays(45);
            lds[1].OrigLearnStartDate = lds[0].LearnStartDate;
            lds[1].OrigLearnStartDateSpecified = true;
            Helpers.SetLearningDeliveryEndDates(lds[1], lds[1].LearnPlanEndDate, Helpers.SetAchDate.DoNotSetAchDate);
            _outcomeDate = lds[1].LearnPlanEndDate;
        }

        private void Mutate19LD2RestartsDestAndProg(MessageLearnerDestinationandProgression learner, bool valid)
        {
            learner.DPOutcome[0].OutStartDate = _outcomeDate;
        }

        private void Mutate16ApprenticeshipComplexRestartsDestAndProg(MessageLearnerDestinationandProgression learner, bool valid)
        {
            learner.DPOutcome[0].OutStartDate = _outcomeDate;
        }

        private void MutateOlderApprenticeshipComplexRestartsDestAndProg(MessageLearnerDestinationandProgression learner, bool valid)
        {
            learner.DPOutcome[0].OutStartDate = _outcomeDate;
        }

        private void MutateGenerationOptions(GenerationOptions options)
        {
            _options = options;
        }

        private void MutateGenerationOptionsLD3(GenerationOptions options)
        {
            options.LD.GenerateMultipleLDs = 3;
            _options = options;
        }

        private void MutateGenerationOptionsLD2(GenerationOptions options)
        {
            options.LD.GenerateMultipleLDs = 2;
            options.LD.OverrideLearnStartDate = DateTime.Parse("2017-AUG-11");
            options.CreateDestinationAndProgression = true;
            _options = options;
        }

        private void MutateGenerationOptionsOlderApprenticeship(GenerationOptions options)
        {
            _options = options;
            options.LD.IncludeHHS = true;
        }

        private void MutateGenerationOptionsOlderApprenticeshipLD2(GenerationOptions options)
        {
            _options = options;
            options.LD.IncludeHHS = true;
            options.LD.GenerateMultipleLDs = 2;
        }

        private void MutateGenerationOptionsOlderApprenticeshipLD3(GenerationOptions options)
        {
            _options = options;
            options.LD.IncludeHHS = true;
            options.LD.GenerateMultipleLDs = 3;
            options.CreateDestinationAndProgression = true;
        }

        private void MutateGenerationOptionsOlderApprenticeshipLD10(GenerationOptions options)
        {
            _options = options;
            options.LD.IncludeHHS = true;
            options.LD.GenerateMultipleLDs = 9;
            options.CreateDestinationAndProgression = true;
        }
    }
}
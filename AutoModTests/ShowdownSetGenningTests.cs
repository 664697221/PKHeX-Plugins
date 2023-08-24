﻿using FluentAssertions;
using PKHeX.Core;
using PKHeX.Core.AutoMod;
using System.Diagnostics;
using Xunit;

namespace AutoModTests
{
    public static class ShowdownSetGenningTests
    {
        static ShowdownSetGenningTests() => TestUtil.InitializePKHeXEnvironment();

        [Theory]
        [InlineData(GameVersion.US, Meowstic)]
        [InlineData(GameVersion.US, Darkrai)]
        [InlineData(GameVersion.B2, Genesect)]
        [InlineData(GameVersion.BD, problemsolving)]
        public static void VerifyManually(GameVersion game, string txt)
        {
            var dev = APILegality.EnableDevMode;
            APILegality.EnableDevMode = true;

            var sav = SaveUtil.GetBlankSAV(game, "ALM");
            TrainerSettings.Register(sav);

            var trainer = TrainerSettings.GetSavedTrainerData(game.GetGeneration(), game);
            RecentTrainerCache.SetRecentTrainer(trainer);

            var set = new ShowdownSet(txt);
            var pkm = sav.GetLegalFromSet(set, out _);
            APILegality.EnableDevMode = dev;

            var la = new LegalityAnalysis(pkm);
            if (!la.Valid)
                Debug.WriteLine(la.Report() + "\n");
            la.Valid.Should().BeTrue();
        }
        private const string problemsolving =
            @"ポッちゃん (Magikarp) (F) @ Lum Berry
IVs: 3 HP / 3 Atk / 11 SpA / 3 SpD / 2 Spe
Ability: Swift Swim
Level: 45
Mild Nature
Language: German
=OT_Name=マイスター
=Met_Location=30001
.Version=48
~=Generation=8
- Splash";
        private const string Darkrai =
@"Darkrai
IVs: 7 Atk
Ability: Bad Dreams
Shiny: Yes
Timid Nature
- Hypnosis
- Feint Attack
- Nightmare
- Double Team";

        private const string Genesect =
@"Genesect
Ability: Download
Shiny: Yes
Hasty Nature
- Extreme Speed
- Techno Blast
- Blaze Kick
- Shift Gear";

        private const string Meowstic =
@"Meowstic-F @ Life Orb
Ability: Competitive
EVs: 4 Def / 252 SpA / 252 Spe
Timid Nature
- Psyshock
- Signal Beam
- Hidden Power Ground
- Calm Mind";
    }
}

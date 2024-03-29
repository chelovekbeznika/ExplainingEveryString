﻿using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Core.Notifications;
using ExplainingEveryString.Core.Timers;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameState
{
    internal class ComponentsManager
    {
        private const String CutsceneSong = "cutscenes";
        private readonly EesGame game;
        private readonly Dictionary<String, CutsceneSpecification> cutscenesMetadata;

        internal InterfaceComponent Interface { get; private set; }
        internal MenuComponent Menu { get; private set; }
        internal GameplayComponent CurrentGameplay { get; private set; }

        internal MultiFrameCutsceneComponent CutsceneBeforeLevel { get; private set; }
        internal LevelTitleComponent CurrentLevelTitle { get; private set; }
        internal LevelEndingComponent CurrentLevelEnding { get; private set; }
        internal MultiFrameCutsceneComponent CutsceneAfterLevel { get; private set; }
        internal TimeAttackResultsComponent TimeAttackResultsComponent { get; private set; }
        internal StaticImagesSequenceComponent MenuCutscene { get; private set; }

        internal MusicComponent MenuMusic { get; private set; }
        internal MusicComponent GameMusic { get; private set; }
        internal MusicComponent CutsceneMusic { get; private set; }
        internal NotificationsComponent Notifications { get; private set; }

        internal ComponentsManager(EesGame game, LevelSequenceSpecification levelSequenceSpecification, 
            Dictionary<String, CutsceneSpecification> cutscenesMetadata, MusicTestButtonSpecification[] musicTestSpecification)
        {
            this.game = game;
            this.cutscenesMetadata = cutscenesMetadata;
            Interface = new InterfaceComponent(game);
            Menu = new MenuComponent(game, levelSequenceSpecification, musicTestSpecification);
            MenuMusic = new MusicComponent(game) { Enabled = false };
            GameMusic = new MusicComponent(game) { Enabled = false };
            CutsceneMusic = new MusicComponent(game) { Enabled = false };
            Notifications = new NotificationsComponent(game);
        }

        internal void InitNewLevelRelatedComponents(String levelName, LevelProgress levelProgress, LevelSequence levelSequence)
        {
            CurrentGameplay = new GameplayComponent(game, levelName, levelProgress);
            Interface.SetGameplayComponentToDraw(CurrentGameplay);
            CurrentLevelTitle = new LevelTitleComponent(game, levelSequence);
            CurrentLevelEnding = new LevelEndingComponent(game, levelSequence);
            TimeAttackResultsComponent = new TimeAttackResultsComponent(game, levelSequence.Specification);
            game.Components.Add(CurrentLevelTitle);
            game.Components.Add(CurrentGameplay);
            game.Components.Add(CurrentLevelEnding);
            game.Components.Add(TimeAttackResultsComponent);
            InitCutscenes(levelSequence);
        }

        private void InitCutscenes(LevelSequence levelSequence)
        {
            var (cutsceneBefore, cutsceneAfter) = levelSequence.GetCurrentLevelCutscenes();
            if (cutsceneBefore != null)
            {
                var metadata = cutscenesMetadata[cutsceneBefore];
                CutsceneBeforeLevel = new MultiFrameCutsceneComponent(game, cutsceneBefore, metadata);
                game.Components.Add(CutsceneBeforeLevel);
            }
            if (cutsceneAfter != null)
            {
                var metadata = cutscenesMetadata[cutsceneAfter];
                CutsceneAfterLevel = new MultiFrameCutsceneComponent(game, cutsceneAfter, metadata);
                game.Components.Add(CutsceneAfterLevel);
            }
        }

        internal void InitTutorialInMenu(String tutorialCutsceneName)
        {
            var metadata = cutscenesMetadata[tutorialCutsceneName];
            MenuCutscene = new MultiFrameCutsceneComponent(game, tutorialCutsceneName, metadata);
            game.Components.Add(MenuCutscene);
        }

        internal void InitTimeTableInMenu(LevelSequenceSpecification levelSequenceSpecification)
        {
            MenuCutscene = new TimeAttackResultsComponent(game, levelSequenceSpecification);
            game.Components.Add(MenuCutscene);
        }

        internal void DeleteMenuCutscene()
        {
            if (MenuCutscene != null)
                game.Components.Remove(MenuCutscene);
        }

        internal void DeleteCurrentLevelRelatedComponents()
        {
            if (CurrentGameplay != null)
            {
                game.Components.Remove(CurrentGameplay);
                Interface.SetGameplayComponentToDraw(null);
                CurrentGameplay = null;
            }
            if (CurrentLevelTitle != null)
            {
                game.Components.Remove(CurrentLevelTitle);
                CurrentLevelTitle = null;
            }
            if (CurrentLevelEnding != null)
            {
                game.Components.Remove(CurrentLevelEnding);
                CurrentLevelEnding = null;
            }
            if (CutsceneBeforeLevel != null)
            {
                game.Components.Remove(CutsceneBeforeLevel);
                CutsceneBeforeLevel = null;
            }
            if (CutsceneAfterLevel != null)
            {
                game.Components.Remove(CutsceneAfterLevel);
                CutsceneAfterLevel = null;
            }
            if (TimeAttackResultsComponent != null)
            {
                game.Components.Remove(TimeAttackResultsComponent);
                TimeAttackResultsComponent = null;
            }
        }

        internal void InitComponents()
        {
            var components = game.Components;
            TimersComponent.Init(game);
            components.Add(Interface);
            components.Add(Menu);
            components.Add(MenuMusic);
            components.Add(GameMusic);
            components.Add(CutsceneMusic);
            components.Add(Notifications);
            SwitchMenuRelatedComponents(true);
        }

        internal void SwitchGameplayRelatedComponents(Boolean active)
        {     
            Interface.Enabled = active; 
            Interface.Visible = active;
            if (CurrentGameplay != null)
            {
                CurrentGameplay.Enabled = active;
                CurrentGameplay.Visible = active;
            }
            GameMusic.Enabled = active;
            TimersComponent.Instance.Enabled = active;
        }

        internal void SwitchCutsceneBeforeLevel(Boolean active)
        {
            if (CutsceneBeforeLevel != null)
            {
                CutsceneBeforeLevel.Enabled = active;
                CutsceneBeforeLevel.Visible = active;
                CutsceneMusic.Enabled = active;
                if (active)
                    CutsceneMusic.PlaySong(CutsceneSong, true);
            }
            else
                CutsceneMusic.Enabled = false;
        }

        internal void SwitchCutsceneAfterLevel(Boolean active)
        {
            if (CutsceneAfterLevel != null)
            {
                CutsceneAfterLevel.Enabled = active;
                CutsceneAfterLevel.Visible = active;
                CutsceneMusic.Enabled = active;
                if (active)
                    CutsceneMusic.PlaySong(CutsceneSong, true);
            }
            else
                CutsceneMusic.Enabled = false;
        }

        internal void SwitchMenuRelatedComponents(Boolean active)
        {
            Menu.Enabled = active;
            Menu.Visible = active;
            MenuMusic.Enabled = active;
        }

        internal void SwitchLevelTitleRelatedComponents(Boolean active)
        {
            if (CurrentLevelTitle != null)
            {
                CurrentLevelTitle.Enabled = active;
                CurrentLevelTitle.Visible = active;
            }
        }

        internal void SwitchLevelEndingRelatedComponents(Boolean active)
        {
            if (CurrentLevelEnding != null)
            {
                CurrentLevelEnding.Enabled = active;
                CurrentLevelEnding.Visible = active;
            }
        }

        internal void SwitchTimeAttackResultsComponents(Boolean active)
        {
            if (TimeAttackResultsComponent != null)
            {
                TimeAttackResultsComponent.Enabled = active;
                TimeAttackResultsComponent.Visible = active;
            }
        }

        internal void SwitchMenuCutsceneRelatedComponents(Boolean active)
        {
            if (MenuCutscene != null)
            {
                MenuCutscene.Enabled = active;
                MenuCutscene.Visible = active;
            }
        }
    }
}

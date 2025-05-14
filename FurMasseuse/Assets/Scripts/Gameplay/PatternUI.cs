using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PatternUI : MonoBehaviour
    {
        [SerializeField]
        private PatternPipUI patternPipPrefab;

        [SerializeField]
        private MainGameplay mainGameplay;

        [SerializeField]
        private Transform patternPipParent;

        private readonly List<PatternPipUI> patternPips = new();

        private void Start()
        {
            mainGameplay.OnPatternCreated.AddListener(HandlePatternCreated);
            mainGameplay.OnStepCompleted.AddListener(HandleStepCompleted);
        }

        private void OnDestroy()
        {
            mainGameplay.OnPatternCreated.RemoveListener(HandlePatternCreated);
            mainGameplay.OnStepCompleted.RemoveListener(HandleStepCompleted);
        }

        private void HandleStepCompleted(int currentFinishedIndex)
        {
            for (var i = 0; i < patternPips.Count; i++)
            {
                if (i < currentFinishedIndex)
                {
                    patternPips[i].SetState(PatternPipUI.State.Finished);
                }
                else
                {
                    patternPips[i].SetState(PatternPipUI.State.Unfinished);
                }
            }
        }

        private void HandlePatternCreated(List<MainGameplay.MassageStrength> patterns)
        {
            if (patterns.Count > patternPips.Count)
            {
                for (int i = patternPips.Count; i < patterns.Count; i++)
                {
                    PatternPipUI pip = Instantiate(patternPipPrefab, patternPipParent);
                    patternPips.Add(pip);
                    pip.Initialize();
                }
            }

            for (var i = 0; i < patternPips.Count; i++)
            {
                patternPips[i].SetType(patterns[i]);
                patternPips[i].SetState(PatternPipUI.State.Unfinished);
            }
        }
    }
}
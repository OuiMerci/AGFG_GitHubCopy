using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AGFG.Core
{
    public abstract class ActivityBase : Selectable
    {
        public abstract void DetachAI(AICharacterBase ai);
        public abstract void BeginActivityIfTypeValid(AICharacterBase ai);
        public abstract bool IsValidAIType(AICharacterBase ai);
        public bool IsActive { get; set; } = true;
        public virtual bool RequiresGoTo => true;
        public abstract void DisableActivity();
        public virtual bool ValidForCCharacterInteraction => true;
    }

    public class ActivityBase<T> : ActivityBase where T : AICharacterBase
    {
        public HashSet<T> AttachedCharacters { get; set;} = new HashSet<T>();
        public bool PlayerCharacterOnly { get; set;} = true;

        public GameObject UIFadingFeedbackPrefab;
        public float CheckDelay;

        private float lastCheck;
        private bool detachingAll;
        [SerializeField] private Canvas canvas;

        protected override void Awake()
        {
            base.Awake();
            if(canvas == null)
                canvas = GetComponentInChildren<Canvas>();
        }

        protected virtual void Update()
        {
        }

        public virtual void BeginActivity(T aiBase)
        {
            if(aiBase is T ai)
            {
                ai.SetCurrentActivity(this);
                AttachedCharacters.Add(ai);
                ai.FaceTarget(transform);
            }
        }

        public override void BeginActivityIfTypeValid(AICharacterBase aiBase)
        {
            if(aiBase is T ai)
                BeginActivity(ai);
        }

        public override bool IsValidAIType(AICharacterBase aiBase)
        {
            return aiBase is T;
        }


        public override void HandleTarget(Selectable target)
        {
            if(target is T character)
            {
                character.HandleTarget(this);
            }
        }

        protected virtual bool CheckReady()
        {
            if(CheckDelay + lastCheck < Time.time)
            {
                lastCheck = Time.time;
                return true;
            }
            
            return false;
        }

        public GameObject SpawnFadingFeedback(string feedbackText)
        {
            if(UIFadingFeedbackPrefab == null)
            {
                Debug.LogError($"[ActivityBase:] Activity does not have a reference to UIFadingFeedback prefab : {name}");
                return null;
            }

            GameObject fadingFeedback = GameObject.Instantiate(UIFadingFeedbackPrefab, canvas.transform);
            fadingFeedback.GetComponent<TextMeshProUGUI>().text = feedbackText;

            return fadingFeedback;
        }

        public override void DetachAI(AICharacterBase aiBase)
        {
            if(aiBase is T ai)
            {
                ai.HandleActivityDetached(this);

                if (detachingAll == false)
                    AttachedCharacters.Remove(ai);
            }
        }

        protected virtual void DetachAll()
        {
            detachingAll = true;
            foreach(T character in AttachedCharacters)
            {
                DetachAI(character);
            }
            AttachedCharacters.Clear();
            detachingAll = false;
        }

        public override void DisableActivity()
        {
            IsActive = false;
            gameObject.SetActive(false);
            DetachAll();
        }
    } 
}

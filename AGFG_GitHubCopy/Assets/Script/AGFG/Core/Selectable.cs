using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Core
{
    public abstract class Selectable : MonoBehaviour
    {
        public bool IsSelected { get; private set;}
        public bool AllowsMultipleSelection { get; protected set; } = false;

        public GameObject SelectionMarker { get; protected set;}

        protected virtual void Awake()
        {
            TryGetSelectionMarker();
        }

        protected virtual void Start()
        {
            SetDeselected();
        }

        public virtual void SetSelected()
        {
            IsSelected = true;
            SelectionMarker.SetActive(true);
        }

        public void SetDeselected()
        {
            IsSelected = false;
            SelectionMarker?.SetActive(false);
        }

        public abstract void HandleTarget(Selectable target);

        protected void TryGetSelectionMarker()
        {
            if(SelectionMarker != null) return;

            if(transform.childCount > 0)
                SelectionMarker = transform.GetChild(0).gameObject;

            if (SelectionMarker != null && SelectionMarker.tag != "SelectionMarker")
            {
                Debug.LogError($"Selectable {gameObject.name} has no SelectionMarker and first children does not seem compatible.");
                SelectionMarker = null;
            }
        }
    } 
}

using System.Collections.Generic;
using UnityEngine;
using AGFG.Core;

namespace AGFG.AGFGInput
{
    public class SelectionInputManager : MonoBehaviour
    {
        public List<Selectable> CurrentSelectedObjects;// { get; private set;}

        // Box selection
        [SerializeField] private RectTransform selectionBoxVisual;
        private Rect selectionBoxLogic;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private Vector2 boxExtents;
        private Vector2 boxCenter;
        private Camera cam;

        private void Start()
        {
            CurrentSelectedObjects = new List<Selectable>();
            cam = Camera.main;
            startPosition = endPosition = boxExtents = boxCenter = Vector2.zero;
        }

        public void Update()
        {
            CheckLeftClickDown();
            CheckLeftClickHold();
            CheckLeftClickUp();
            CheckRightClick();
        }

        public void CheckLeftClickDown()
        {
            if(Input.GetMouseButtonDown(0))
            {
                var selected = TryRaycastToSelectable(out RaycastHit hit);

                if (Input.GetKey(KeyCode.LeftShift)) // Left shift == multiple selection
                {
                    if (selected != null && selected.AllowsMultipleSelection)
                    {
                        AddToSelection(selected);
                    }
                }
                else
                {
                    SetNewSelected(selected);
                }

                // For box selection
                startPosition = Input.mousePosition;
                selectionBoxLogic = new Rect();
            }
        }
        private void CheckLeftClickHold()
        {
            if(Input.GetMouseButton(0))
            {
                endPosition = Input.mousePosition;
                DrawVisual();
                DrawSelection();
            }
        }

        private void CheckLeftClickUp()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SelectUnits();
                startPosition = endPosition = boxExtents = boxCenter = Vector2.zero;
                DrawVisual(); // reset visual to empty rect
            }
        }

        private void CheckRightClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var clicked = TryRaycastToSelectable(out RaycastHit hit);
                
                if(clicked != null)
                {
                    CurrentSelectedObjects?.ForEach(o => o.HandleTarget(clicked));
                }
                else if(hit.collider.gameObject.layer == 6) // Ground layermask
                {
                    TryApplyMovementRequest(hit.point);
                }
            }
        }

        private Selectable TryRaycastToSelectable(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                return hit.transform.GetComponent<Selectable>(); // might be null
            }

            Debug.Log("[SelectionManager] No object hit from click raycast");

            return null;
        }

        private void AddToSelection(Selectable newSelected)
        {
            if(newSelected != null && CurrentSelectedObjects.Contains(newSelected) == false)
            {
                CurrentSelectedObjects.Add(newSelected);
                newSelected.SetSelected();
            }
        }

        private void SetNewSelected(Selectable newSelected)
        {
            DeselectAll();

            if(newSelected != null)
            {
                CurrentSelectedObjects.Add(newSelected);
                newSelected.SetSelected();
            }
        }

        private void DeselectAll()
        {
            CurrentSelectedObjects.ForEach(o => o.SetDeselected());
            CurrentSelectedObjects.Clear();
        }

        private void DrawVisual()
        {
            boxCenter = (startPosition + endPosition) / 2;
            selectionBoxVisual.position = boxCenter;

            Vector2 boxSize = new Vector2(Mathf.Abs(startPosition.x - endPosition.x), Mathf.Abs(startPosition.y - endPosition.y));
            selectionBoxVisual.sizeDelta = boxSize;
            boxExtents = boxSize / 2.0f;
        }

        private void DrawSelection()
        {
            selectionBoxLogic.min = boxCenter - boxExtents;
            selectionBoxLogic.max = boxCenter + boxExtents;
        }

        private void SelectUnits()
        {
            foreach (AICCharacter ai in AICCharacter.s_AICharacterList)
            {
                var characterScreenPoint = cam.WorldToScreenPoint(ai.transform.position);
                if(selectionBoxLogic.Contains(characterScreenPoint))
                {
                    AddToSelection(ai);
                }
            }
        }

        private void TryApplyMovementRequest(Vector3 destination)
        {
            foreach(Selectable s in CurrentSelectedObjects)
            {
                if(s is AICCharacter ai)
                {
                    ai.HandleGoToRequest(destination);
                }
            };
        }
    }

}
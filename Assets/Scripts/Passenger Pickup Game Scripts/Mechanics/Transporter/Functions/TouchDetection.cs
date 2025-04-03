using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class TouchDetection : MonoBehaviour
    {
        private bool _isTouching = false;

        TransporterMovement _transporterMovement;

        private void OnEnable()
        {
            _transporterMovement = GetComponentInParent<TransporterMovement>();
        }

        private void OnDisable()
        {
            _transporterMovement = null;
        }

        void OnMouseDown()
        {
            if (!_isTouching)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    _isTouching = true;
                    _transporterMovement._IsReversed = GetComponent<WagonData>().IsTailWagon;
                    _transporterMovement._IsSelected = true;
                    Debug.Log(hit.collider.gameObject.name + " tıklandı" + "Id :" + hit.collider.gameObject.GetInstanceID());
                }
            }
        }

        private void OnMouseUp()
        {
            _isTouching = false;
            _transporterMovement._IsReversed = false;
            _transporterMovement._IsSelected = false;
        }
    }
}

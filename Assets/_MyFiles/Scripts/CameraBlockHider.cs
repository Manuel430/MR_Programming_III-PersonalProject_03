using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
    public class CameraBlockHider : MonoBehaviour
    {
        [SerializeField] Transform playerTransform;
        [SerializeField] float traceOffset = -2;
        [SerializeField] float traceStartOffset = -20;
        List<GameObject> hidingGameObjects = new List<GameObject>();

        private void Update()
        {
            Vector3 playerLocation = playerTransform.position;
            Vector3 checkLocation = transform.position + transform.forward * traceStartOffset;

            float checkDistance = Vector3.Distance(playerLocation, checkLocation) + traceOffset;
            RaycastHit[] allBlockingHits= Physics.RaycastAll(checkLocation, (playerLocation- checkLocation).normalized, checkDistance);

            List<GameObject> newBlockingGameObjects = new List<GameObject>();
            foreach(RaycastHit hit in allBlockingHits )
            {
                GameObject newBlockingGameObject = hit.collider.gameObject;
                if (newBlockingGameObject != playerTransform.gameObject)
                { 
                    newBlockingGameObjects.Add(hit.collider.gameObject);
                    SeGameObjectVisiblity(newBlockingGameObject, false);
                }
                
            }

            foreach(GameObject hidenGameObject in hidingGameObjects)
            {
                if(!newBlockingGameObjects.Contains(hidenGameObject))
                {
                    SeGameObjectVisiblity(hidenGameObject, true);
                }
            }

            hidingGameObjects = newBlockingGameObjects;
        }

        void SeGameObjectVisiblity(GameObject gameObj, bool visible)
        {
            var renders = gameObj.GetComponentsInChildren<Renderer>();
            foreach(var render in renders)
            {
                render.enabled = visible;
            }
        }

        private void OnDrawGizmos()
        {

            Vector3 playerLocation = playerTransform.position;
            Vector3 checkLocation = transform.position + transform.forward * traceStartOffset;


            Gizmos.DrawLine(checkLocation, playerLocation);    
        }
    }
}

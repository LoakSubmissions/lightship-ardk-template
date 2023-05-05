using System;
using System.Collections;
using System.Collections.Generic;
using Loak.Unity;
using UnityEngine;

namespace Loak.Examples
{
    public class SharedARExample : MonoBehaviour
    {
        private Dictionary<Guid, GameObject> peerObjects = new Dictionary<Guid, GameObject>();

        public void SendPlace(GameObject obj)
        {
            LoakSessionManager.Instance.SendToHost(LoakTapPlace.Instance.allowMultiple ? (uint)4 : (uint)5, obj.transform.position);
        }

        public void OnDataRecieved(uint tag, Guid sender, object[] data)
        {
            switch (tag)
            {
                case 4:
                    peerObjects[sender] = Instantiate(LoakTapPlace.Instance.objectToPlace, (Vector3)data[0], Quaternion.identity, LoakTapPlace.Instance.objectParent);
                    peerObjects[sender].SetActive(true);

                    if (LoakSessionManager.Instance.IsHost)
                        LoakSessionManager.Instance.SendToAll(4, sender, (Vector3)data[0]);

                    break;
                
                case 5:
                    GameObject instance;

                    if (!peerObjects.TryGetValue(sender, out instance))
                        instance = Instantiate(LoakTapPlace.Instance.objectToPlace, LoakTapPlace.Instance.objectParent);

                    instance.transform.position = (Vector3)data[0];

                    if (LoakSessionManager.Instance.IsHost)
                        LoakSessionManager.Instance.SendToAll(5, sender, (Vector3)data[0]);

                    break;
            }
        }
    }
}

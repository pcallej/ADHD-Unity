using UnityEngine;
using System.Collections.Generic;
using System;

namespace MalbersAnimations.Utilities
{
    [Serializable]
    public class MeshBlendShapes
    {
        public string NameID;
        public SkinnedMeshRenderer mesh;
        [Range(0, 100)]
        public float[] blendShapes;     //Value of the Blend Shape
        public bool[] ActiveShape;      //if this Blend Shape Active?

        public virtual void UpdateBlendShapes()
        {
            if (mesh != null && blendShapes != null)
            {
                if (NameID == string.Empty) NameID = mesh.name;

                if (blendShapes.Length != mesh.sharedMesh.blendShapeCount)
                {
                    blendShapes = new float[mesh.sharedMesh.blendShapeCount];
                }

                for (int i = 0; i < blendShapes.Length; i++)
                {
                    mesh.SetBlendShapeWeight(i, blendShapes[i]);
                }
            }
        }

        public virtual float[] GetBlendShapeValues()
        {
            if (mesh && mesh.sharedMesh.blendShapeCount > 0)
            {
                float[] BS = new float[mesh.sharedMesh.blendShapeCount];

                for (int i = 0; i < BS.Length; i++)
                {
                    BS[i] = mesh.GetBlendShapeWeight(i);
                }
                return BS;
            }
            return null;
        }
    }

    public class BlendShapes : MonoBehaviour
    {
        [SerializeField]
        public List<MeshBlendShapes> Shapes;

        public virtual void UpdateBlendShapes()
        {
            foreach (MeshBlendShapes item in Shapes)
            {
                item.UpdateBlendShapes();
            }
        }
    }
}
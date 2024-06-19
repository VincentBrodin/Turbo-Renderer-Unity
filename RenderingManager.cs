using System;
using System.Collections.Generic;
using Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace TurboRenderer{
    public class RenderingManager : MonoSingleton<RenderingManager>{
        [Serializable]
        public class Group{
            [FormerlySerializedAs("id")]
            public string groupId;
            public Material material;
            public Mesh mesh;
            private Dictionary<int, Matrix4x4> _matrix = new();
            private int _idCount = -1;


            public int Add(Matrix4x4 matrix4X4){
                _idCount++;
                _matrix.Add(_idCount, matrix4X4);
                return _idCount;
            }
            
            public void Add(int id, Matrix4x4 matrix4X4){
                _matrix.Add(id, matrix4X4);
            }
            
            public void Remove(int id){
                _matrix.Remove(id);
            }

            public void UpdateMatrix(int id, Matrix4x4 newMatrix){
                _matrix[id] = newMatrix;
            }

            public IEnumerable<Matrix4x4> Get(){
                return _matrix.Values;
            }
        }

        [SerializeField] private List<Group> groups;
        private readonly Dictionary<string, Group> _groups = new();


        private void OnEnable(){
            foreach (Group type in groups){
                _groups.Add(type.groupId, type);
            }
        }

        public int Add(string groupId, Matrix4x4 matrix4X4){
            return _groups[groupId].Add(matrix4X4);
        }
        
        public void Add(string groupId, int id, Matrix4x4 matrix4X4){
            _groups[groupId].Add(id, matrix4X4);
        }
        
        public void Remove(string groupId, int id){
            _groups[groupId].Remove(id);
        }

        public void UpdateMatrix(string groupId, int id, Matrix4x4 matrix4X4){
            _groups[groupId].UpdateMatrix(id, matrix4X4);
        }

        private void Update(){
            foreach (Group group in _groups.Values){
                Graphics.DrawMeshInstanced(group.mesh, 0, group.material, group.Get().ToListPooled());
            }
        }
    }
}

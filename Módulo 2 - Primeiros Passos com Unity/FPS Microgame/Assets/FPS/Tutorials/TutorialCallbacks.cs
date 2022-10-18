using UnityEngine;
using Unity.Tutorials.Core.Editor;
using UnityEditor;
using UnityEngine.AI;

namespace Unity.Tutorials
{
    /// <summary>
    /// Implement your Tutorial callbacks here.
    /// </summary>
    public class TutorialCallbacks : ScriptableObject
    {
        public FutureObjectReference futureRoomInstance = default;
        public FutureObjectReference futureBotInstance = default;
        NavMeshSurface navMeshSurface = default;

        public bool NavMeshIsBuilt()
        {
            return navMeshSurface.navMeshData != null;
        }

        public void ClearAllNavMeshes()
        {
            if (!navMeshSurface)
            {
                navMeshSurface = GameObject.FindObjectOfType<NavMeshSurface>();
            }
            UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
            navMeshSurface.navMeshData = null;
        }

        /// <summary>
        /// Keeps the Room selected during a tutorial. 
        /// </summary>
        public void KeepRoomSelected()
        {
            SelectSpawnedGameObject(futureRoomInstance);
        }

        /// <summary>
        /// Keeps the Room selected during a tutorial. 
        /// </summary>
        public void KeepBotSelected()
        {
            SelectSpawnedGameObject(futureBotInstance);
        }


        /// <summary>
        /// Selects a GameObject in the scene, marking it as the active object for selection
        /// </summary>
        /// <param name="futureObjectReference"></param>
        public void SelectSpawnedGameObject(FutureObjectReference futureObjectReference)
        {
            if (futureObjectReference.SceneObjectReference == null) { return; }
            Selection.activeObject = futureObjectReference.SceneObjectReference.ReferencedObjectAsGameObject;
        }

        public void SelectMoveTool()
        {
            Tools.current = Tool.Move;
        }

        public void SelectRotateTool()
        {
            Tools.current = Tool.Rotate;
        }

        public void StartTutorial(Tutorial tutorial)
        {
            TutorialManager.Instance.StartTutorial(tutorial);
        }
    }
}
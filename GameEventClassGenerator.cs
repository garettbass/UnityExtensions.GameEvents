#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace UnityExtensions
{
    public static class GameEventClassGenerator
    {

        public const int MenuPriority = 0;

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            GenerateGameEventClasses(
                logThresholdMs: 64,
                skipCoreGameEventClasses: true);
        }

        [MenuItem(
            "Assets/Create/GameEvent/Generate GameEvent Classes",
            priority = MenuPriority + 128)]
        private static void GenerateGameEventClassesMenuItem()
        {
            GenerateGameEventClasses(
                logThresholdMs: 0,
                skipCoreGameEventClasses: false);
        }

        private static void GenerateGameEventClasses(
            double logThresholdMs = 0,
            bool skipCoreGameEventClasses = false)
        {
            var start = DateTime.Now;
            GenerateCoreGameEventClasses();
            // GenerateGameEventMessageAttributedClasses();
            var elapsedMs = (DateTime.Now - start).TotalMilliseconds;
            if (elapsedMs > logThresholdMs)
                Debug.Log($"GenerateGameEventClasses() took {elapsedMs} ms.");
        }

        private static void GenerateCoreGameEventClasses()
        {
            var filePath =
                new StackTrace(fNeedFileInfo: true)
                .GetFrame(0)
                .GetFileName();
            var fileDirectory =
                Path.GetDirectoryName(filePath);
            var generatedDirectory =
                Path.Combine(fileDirectory, "Generated");
            GenerateGameEventClasses<Color>(generatedDirectory);
            GenerateGameEventClasses<float>(generatedDirectory, "Float");
            GenerateGameEventClasses<GameObject>(generatedDirectory);
            GenerateGameEventClasses<int>(generatedDirectory, "Integer");
            GenerateGameEventClasses<Object>(generatedDirectory);
            GenerateGameEventClasses<PointerEventData>(generatedDirectory);
            GenerateGameEventClasses<RaycastHit>(generatedDirectory);
            GenerateGameEventClasses<RaycastResult>(generatedDirectory);
            GenerateGameEventClasses<Sprite>(generatedDirectory);
            GenerateGameEventClasses<string>(generatedDirectory);
            GenerateGameEventClasses<Transform>(generatedDirectory);
            GenerateGameEventClasses<Vector2>(generatedDirectory);
            GenerateGameEventClasses<Vector3>(generatedDirectory);
            GenerateGameEventClasses<Vector4>(generatedDirectory);
        }

        public static void GenerateGameEventMessageAttributedClasses()
        {
            // var notificationMessageAttributes =
            //     GameEventMessageAttribute.FindAll();

            // foreach (var attribute in notificationMessageAttributes)
            //     GenerateGameEventClasses(
            //         attribute.notificationClassDirectory,
            //         attribute.notificationMessageType);
        }

        public static void GenerateGameEventClasses<T>(
            string directory,
            string messageName = null)
        {
            GenerateGameEventClasses(directory, typeof(T), messageName);
        }

        public static void GenerateGameEventClasses(
            string directory,
            Type messageType,
            string messageName = null)
        {
            var assetsRoot = "Assets/";
            if (directory.StartsWith(assetsRoot))
                directory = directory.Substring(assetsRoot.Length);
            var dataPath = Application.dataPath;
            if (directory.StartsWith(dataPath) == false)
                directory = Path.Combine(dataPath, directory);
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            if (messageName == null)
                messageName = messageType.Name;

            // Debug.Log($"generate {directory}/{messageName}...");
            GenerateGameEvent(directory, messageType, messageName);
            GenerateGameEventAction(directory, messageType, messageName);
            GenerateGameEventRouter(directory, messageType, messageName);
        }

        private static void GenerateGameEvent(
            string directory,
            Type messageType,
            string messageName)
        {
            var ns = messageType.Namespace;
            var fullName = messageType.FullName;
            var fileName = $"{messageName}GameEvent.cs";
            var filePath = Path.Combine(directory, fileName);
            if (File.Exists(filePath))
                return;

            var order = MenuPriority + (int)messageName[0];
            var source =
                $"// Generated by GameEventClassGenerator\n" +
                $"namespace {messageType.Namespace} {{\n\n"+
                $"    [UnityEngine.CreateAssetMenu(\n" +
                $"        menuName =\"GameEvent/{messageName} GameEvent\",\n" +
                $"        order = {order})]\n" +
                $"    public class {messageName}GameEvent :\n" +
                $"        UnityExtensions.GameEvent<{fullName}>\n" +
                $"    {{ }}\n\n"+
                $"}}";

            File.WriteAllText(filePath, source);
            RefreshAssetDatabaseOnNextEditorUpdate();
        }

        private static void GenerateGameEventAction(
            string directory,
            Type messageType,
            string messageName)
        {
            var fullName = messageType.FullName;
            var fileName = $"{messageName}GameEventAction.cs";
            var filePath = Path.Combine(directory, fileName);
            if (File.Exists(filePath))
                return;

            var source =
                $"// Generated by GameEventClassGenerator\n" +
                $"namespace {messageType.Namespace} {{\n\n"+
                $"    [System.Serializable]\n" +
                $"    public class {messageName}GameEventAction :\n" +
                $"        UnityExtensions.GameEventAction<{fullName}>\n" +
                $"    {{ }}"+
                $"}}";

            File.WriteAllText(filePath, source);
            RefreshAssetDatabaseOnNextEditorUpdate();
        }

        private static void GenerateGameEventRouter(
            string directory,
            Type messageType,
            string messageName)
        {
            var fullName = messageType.FullName;
            var fileName = $"{messageName}GameEventRouter.cs";
            var filePath = Path.Combine(directory, fileName);
            if (File.Exists(filePath))
                return;

            var source =
                $"// Generated by GameEventClassGenerator\n" +
                $"namespace {messageType.Namespace} {{\n\n"+
                $"    public class {messageName}GameEventRouter :\n" +
                $"        UnityExtensions.GameEventRouter<\n" +
                $"            {fullName},\n" +
                $"            {messageName}GameEvent,\n" +
                $"            {messageName}GameEventAction>\n" +
                $"    {{ }}"+
                $"}}";

            File.WriteAllText(filePath, source);
            RefreshAssetDatabaseOnNextEditorUpdate();
        }

        private static void RefreshAssetDatabaseOnNextEditorUpdate()
        {
            EditorApplication.delayCall -= AssetDatabase.Refresh;
            EditorApplication.delayCall += AssetDatabase.Refresh;
        }

    }

}
#endif // UNITY_EDITOR
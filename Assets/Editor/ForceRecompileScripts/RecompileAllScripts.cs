using UnityEditor;

namespace ElysiumEditor
{
	public class RecompileAllScripts : EditorWindow
	{
		[MenuItem("Graph View/Recompile All Scripts")]
		public static void RecompileScripts()
		{
			UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
		}
	}
}

using UnityEditor;
using UnityEditor.UI;

namespace UnitySampleAssets.CrossPlatformInput.Inspector
{
	[CustomEditor(typeof (InputAxisScrollbar))]
	public class InputAxisScrollbarCI : ScrollbarEditor
	{
		public override void OnInspectorGUI()
		{
			InputAxisScrollbar awesomescrollbar = target as InputAxisScrollbar;
			awesomescrollbar.axis = EditorGUILayout.TextField("Input Axis", awesomescrollbar.axis);
			base.OnInspectorGUI();
		}
	}
}
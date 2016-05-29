// GUI Animator FREE
// Version: 0.9.95
// Compatilble: Unity 4.6.9 or higher and Unity 5.3.2 or higher, more info in Readme.txt file.
//
// Author:	Gold Experience Team (http://www.ge-team.com)
// Details:	https://www.assetstore.unity3d.com/en/#!/content/28709
// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.

// ######################################################################
// GA_FREE_OpenOtherScene class
// This class handles 8 buttons for changing scene.
// ######################################################################

class GA_FREE_JS_OpenOtherScene extends MonoBehaviour {
	
	// ########################################
	// MonoBehaviour Functions
	// ########################################
		
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    function Start () {
		
    }
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    function Update () {
		
    }
	
	// ########################################
	// UI Responder functions
	// ########################################
		
    // Open Demo Scene 1
    function ButtonOpenDemoScene1 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo01 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
	
    // Open Demo Scene 2
    function ButtonOpenDemoScene2 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo02 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
	
    // Open Demo Scene 3
    function ButtonOpenDemoScene3 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo03 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
	
    // Open Demo Scene 4
    function ButtonOpenDemoScene4 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo04 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
	
    // Open Demo Scene 5
    function ButtonOpenDemoScene5 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo05 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
	
    // Open Demo Scene 6
    function ButtonOpenDemoScene6 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo06 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
	
    // Open Demo Scene 7
    function ButtonOpenDemoScene7 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo07 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
	
    // Open Demo Scene 8
    function ButtonOpenDemoScene8 () {
        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel("GA FREE JS - Demo08 (960x600px)", 1.5f);
		
        gameObject.SendMessage("HideAllGUIs");
    }
}

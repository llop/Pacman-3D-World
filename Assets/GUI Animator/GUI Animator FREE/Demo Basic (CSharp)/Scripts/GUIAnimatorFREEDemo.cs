// GUI Animator FREE
// Version: 0.9.95
// Compatilble: Unity 4.6.9 or higher and Unity 5.3.2 or higher, more info in Readme.txt file.
//
// Author:	Gold Experience Team (http://www.ge-team.com)
// Details:	https://www.assetstore.unity3d.com/en/#!/content/28709
// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

#endregion

// ######################################################################
// Demo class
// This class shows buttons and it plays Move-In and Move-Out animations when user pressed the buttons.
// ######################################################################

public class Demo : MonoBehaviour
{


	// ########################################
	// MonoBehaviour functions
	// ########################################

	#region MonoBehaviour Functions

	private float m_WaitTime = 4.0f;
	private float m_WaitTimeCount = 0;
	private bool m_ShowMoveInButton = true;

	// Use this for initialization
	void Awake()
	{
		// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false, 
		// this will let you control all GUI Animator elements in the scene via scripts.
		if (enabled)
		{
			GUIAnimSystemFREE.Instance.m_GUISpeed = 1.0f;
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}
	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

		// Count down timer for MoveIn/MoveOut buttons
		if (m_WaitTimeCount > 0 && m_WaitTimeCount <= m_WaitTime)
		{
			m_WaitTimeCount -= Time.deltaTime;
			if (m_WaitTimeCount <= 0)
			{
				m_WaitTimeCount = 0;

				// Switch status of m_ShowMoveInButton
				m_ShowMoveInButton = !m_ShowMoveInButton;
			}
		}
	}

	void OnGUI()
	{
		// Show GUI button when ready
		if (m_WaitTimeCount <= 0)
		{
			Rect rect = new Rect((Screen.width - 100) / 2, (Screen.height - 50) / 2, 100, 50);
			// Show MoveIn button
			if (m_ShowMoveInButton == true)
			{
				if (GUI.Button(rect, "MoveIn"))
				{
					// Play MoveIn animations
					GUIAnimSystemFREE.Instance.MoveIn(this.transform, true);
					m_WaitTimeCount = m_WaitTime;
				}
			}
			// Show MoveOut button
			else
			{
				if (GUI.Button(rect, "MoveOut"))
				{
					// Play MoveOut animations
					GUIAnimSystemFREE.Instance.MoveOut(this.transform, true);
					m_WaitTimeCount = m_WaitTime;
				}
			}
		}
	}

	#endregion // MonoBehaviour Functions
}

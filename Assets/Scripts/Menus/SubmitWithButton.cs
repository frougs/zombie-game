using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
  
/// <summary>Submits an InputField with the specified button.</summary>
//Prevents MonoBehaviour of same type (or subtype) to be added more than once to a GameObject.
[DisallowMultipleComponent]
//This automatically adds required components as dependencies.
//[RequireComponent(typeof(InputField))]
public class SubmitWithButton : MonoBehaviour
{
	//public string submitKey = "Return";
	public bool trimWhitespace = true;
    public UnityEvent submitted;

	//Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	//Apropriate when initializing fields.
	void Start() {
		_inputField = GetComponent<TMP_InputField>();
		_inputField.onEndEdit.AddListener(fieldValue => {
			if (trimWhitespace)
				_inputField.text = fieldValue = fieldValue.Trim();
			if (Input.GetKeyDown(KeyCode.Return))
				validateAndSubmit(fieldValue);
		});
	}
	TMP_InputField _inputField;
  
	bool isInvalid(string fieldValue) {
		// change to the validation you want
		return string.IsNullOrEmpty(fieldValue);
	}
	void validateAndSubmit(string fieldValue) {
		if (isInvalid(fieldValue))
			return;
		// change to whatever you want to run when user submits
		submitted?.Invoke();
	}
	// to be called from a submit button onClick event
	public void validateAndSubmit() {
		validateAndSubmit(_inputField.text);
	}
}
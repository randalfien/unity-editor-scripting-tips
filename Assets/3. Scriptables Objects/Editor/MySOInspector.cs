using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
#pragma warning disable 649

/*
 Google Sheets javascript:
 
function doGet(e)
{
  var result;
  
  var sheets = SpreadsheetApp.getActiveSheet();
  var ar = [];
  for( var i = 0; i < 8; i++ ){
    ar.push( sheets.getRange(1+i, 1).getValue() );
  }
  
  var result = {result:ar};
  
  
  var JsonValue = JSON.stringify(result);
  
  return ContentService.createTextOutput(JsonValue.toString()).setMimeType(ContentService.MimeType.JSON);
}
 
 */

[CustomEditor(typeof(MyScriptableObjectScript))]
public class MySOInspector : Editor
{
    private UnityWebRequest _webRequest;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Update from GDrive"))
        {
            _webRequest = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbwDNbVdFmkHwuRNQ9W9KEibmuNFgB0eW3gQV1rnScL2S7hXD8yW/exec");
            _webRequest.SendWebRequest();

            EditorApplication.update += CheckForImportRequestEnd;
        }
    }

    private class GDocResponse
    {
        public int[] result;
    }
    
    private void CheckForImportRequestEnd()
    {
        if (_webRequest != null && _webRequest.isDone)
        {
            var result = JsonUtility.FromJson<GDocResponse>(_webRequest.downloadHandler.text);
            MyScriptableObjectScript myTarget = (MyScriptableObjectScript)target;
            myTarget.Value = result.result;
            EditorApplication.update -= CheckForImportRequestEnd;
            Repaint();
        }
    }
}

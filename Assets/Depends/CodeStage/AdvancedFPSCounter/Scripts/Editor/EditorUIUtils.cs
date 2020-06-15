#if UNITY_EDITOR

using DG.DemiEditor;
using UnityEditor;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.Editor.UI
{
    internal struct EditorUIUtils : System.IDisposable
    {
        private enum LayoutMode : byte
        {
            Horizontal,
            Vertical
        }

        public static GUIStyle line;
        public static GUIStyle panelWithBackground;
        public static GUIStyle richBoldFoldout;
        public static GUIStyle richMiniLabel;
        private readonly LayoutMode mode;

        private EditorUIUtils( LayoutMode layoutMode, GUIStyle style, params GUILayoutOption[] options )
        {
            mode = layoutMode;

            if ( mode == LayoutMode.Horizontal )
            {
                GUILayout.BeginHorizontal( style, options );
            }
            else
            {
                GUILayout.BeginVertical( style, options );
            }
        }

        public static bool DrawProperty( SerializedProperty property, System.Action setter, params GUILayoutOption[] options )
        {
            return DrawProperty( property, (GUIContent)null, setter, options );
        }

        public static bool DrawProperty( SerializedProperty property, string content, System.Action setter, params GUILayoutOption[] options )
        {
            return DrawProperty( property, new GUIContent( content ), setter, options );
        }

        public static bool DrawProperty( SerializedProperty property, GUIContent content, System.Action setter, params GUILayoutOption[] options )
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField( property, content, options );
            if ( EditorGUI.EndChangeCheck() )
            {
                setter.Invoke();
                return true;
            }

            return false;
        }

        public static bool Foldout( SerializedProperty foldout, string caption )
        {
            Separator( 5 );
            GUILayout.BeginHorizontal( panelWithBackground );
            GUILayout.Space( 13 );
            foldout.isExpanded = EditorGUI.Foldout( EditorGUILayout.GetControlRect(), foldout.isExpanded, caption, true, richBoldFoldout );
            GUILayout.EndHorizontal();
            return foldout.isExpanded;
        }

        public static void Header( string header )
        {
            var rect = EditorGUILayout.GetControlRect(false, 24);
            rect.y += 8f;
            rect = EditorGUI.IndentedRect( rect );
            GUI.Label( rect, header, EditorStyles.boldLabel );
        }

        // ----------------------------------------------------------------------------
        // tooling for "using" keyword
        // ----------------------------------------------------------------------------
        public static EditorUIUtils Horizontal( params GUILayoutOption[] options )
        {
            return Horizontal( GUIStyle.none, options );
        }

        public static EditorUIUtils Horizontal( GUIStyle style, params GUILayoutOption[] options )
        {
            return new EditorUIUtils( LayoutMode.Horizontal, style, options );
        }

        public static void Indent( int topPadding = 2, int level = 5 )
        {
            EditorGUI.indentLevel++;
            GUILayout.Space( topPadding );
        }

        public static void Separator( int padding = 0 )
        {
            if ( padding != 0 )
                GUILayout.Space( padding );

            var position = EditorGUILayout.GetControlRect(false, 1f);
            position = EditorGUI.PrefixLabel( position, GUIContent.none );
            var texCoordinates = new Rect(0f, 1f, 1f, 1f);
            //GUI.DrawTextureWithTexCoords( position, line.normal.background, texCoordinates );

            if ( padding != 0 )
                GUILayout.Space( padding );
        }

        public static void SetupStyles()
        {
            if ( richBoldFoldout != null )
                return;

            richBoldFoldout = new GUIStyle( EditorStyles.foldout ) {
                richText = true,
                fontStyle = FontStyle.Bold
            };

            richMiniLabel = new GUIStyle( EditorStyles.miniLabel ) {
                wordWrap = true,
                richText = true
            };

            panelWithBackground = new GUIStyle( GUI.skin.box ) {
                padding = new RectOffset()
            };

            line = GUI.skin.GetStyle( "CN Box" );
        }

        public static bool ToggleFoldout( SerializedProperty toggle, SerializedProperty foldout, string caption, bool bold = true, bool separator = true )
        {
            if ( separator )
                Separator( 5 );

            GUILayout.BeginHorizontal( panelWithBackground );
            EditorGUIUtility.labelWidth = 1;
            EditorGUILayout.PropertyField( toggle, GUIContent.none, GUILayout.ExpandWidth( false ) );
            EditorGUIUtility.labelWidth = 0;

            GUILayout.Space( 10 );
            var rect = EditorGUILayout.GetControlRect();
            foldout.isExpanded = EditorGUI.Foldout( rect, foldout.isExpanded, caption, true, bold ? richBoldFoldout : EditorStyles.foldout );
            GUILayout.EndHorizontal();

            return toggle.boolValue;
        }

        public static void UnIndent( int bottomPadding = 5 )
        {
            EditorGUI.indentLevel--;
            GUILayout.Space( bottomPadding );
        }

        public static EditorUIUtils Vertical( params GUILayoutOption[] options )
        {
            return Vertical( GUIStyle.none, options );
        }

        public static EditorUIUtils Vertical( GUIStyle style, params GUILayoutOption[] options )
        {
            return new EditorUIUtils( LayoutMode.Vertical, style, options );
        }

        public void Dispose()
        {
            if ( mode == LayoutMode.Horizontal )
            {
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.EndVertical();
            }
        }
    }
}

#endif
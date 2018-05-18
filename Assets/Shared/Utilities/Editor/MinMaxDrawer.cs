// NOTE put in a Editor folder

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxAttribute))]
public class MinMaxDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// cast the attribute to make life easier
		MinMaxAttribute minMax = attribute as MinMaxAttribute;

		// This only works on a vector2! ignore on any other property type (we should probably draw an error message instead!)
		if (property.propertyType == SerializedPropertyType.Vector2)
		{


			// if we are flagged to draw in a special mode, lets modify the drawing rectangle to draw only one line at a time
			if (minMax.ShowDebugValues || minMax.ShowEditRange)
			{
				position = new Rect (position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			}
				

//			EditorGUI.PropertyField(position, property, label, true);

//			position.y += EditorGUIUtility.singleLineHeight;

			// pull out a bunch of helpful min/max values....
			float minValue = property.vector2Value.x; // the currently set minimum and maximum value
			float maxValue = property.vector2Value.y;
			float minLimit = minMax.MinLimit; // the limit for both min and max, min cant go lower than minLimit and maax cant top maxLimit
			float maxLimit = minMax.MaxLimit;

//			GUILayout.BeginArea(position);
//
//			GUILayout.BeginHorizontal();
//			EditorGUILayout.LabelField(label);
//
//			GUILayout.BeginVertical();
//
//			GUILayout.BeginHorizontal();
//			EditorGUILayout.FloatField(minValue);
//			EditorGUILayout.FloatField(maxValue);
//			GUILayout.EndHorizontal();
//
//			EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, minLimit, maxLimit);
//			GUILayout.EndVertical();
//			GUILayout.EndHorizontal();
//			GUILayout.EndArea();
//		

			float space = 10f;
			Rect firstLineRect = position;
			firstLineRect.height = EditorGUIUtility.singleLineHeight;
			firstLineRect.width = firstLineRect.width / 3;
			EditorGUI.LabelField(firstLineRect, label.text + ":");

			firstLineRect.x += firstLineRect.width + space;
			firstLineRect.width -= space * 2;

			minValue = EditorGUI.FloatField(firstLineRect, minValue);

			firstLineRect.x += firstLineRect.width + space;
			maxValue = EditorGUI.FloatField(firstLineRect, maxValue);

			// and ask unity to draw them all nice for us!
			position.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, minLimit, maxLimit);

			var vec = Vector2.zero; // save the results into the property!
			vec.x = minValue;
			vec.y = maxValue;

			property.vector2Value = vec;

			// Do we have a special mode flagged? time to draw lines!
			if (minMax.ShowDebugValues || minMax.ShowEditRange)
			{
				bool isEditable = false;
				if (minMax.ShowEditRange)
				{
					isEditable = true;
				}

				if (!isEditable)
					GUI.enabled = false; // if were just in debug mode and not edit mode, make sure all the UI is read only!

				// move the draw rect on by one line
				position.y += EditorGUIUtility.singleLineHeight;

				Vector4 val = new Vector4 (minLimit, minValue, maxValue, maxLimit); // shove the values and limits into a vector4 and draw them all at once
				val = EditorGUI.Vector4Field(position, "MinLimit/MinVal/MaxVal/MaxLimit", val);

				GUI.enabled = false; // the range part is always read only
				position.y += EditorGUIUtility.singleLineHeight;
				EditorGUI.FloatField(position, "Selected Range", maxValue - minValue);
				GUI.enabled = true; // remember to make the UI editable again!

				if (isEditable)
				{
					property.vector2Value = new Vector2 (val.y, val.z); // save off any change to the value~
				}
			}
		}
	}

	// this method lets unity know how big to draw the property. We need to override this because it could end up meing more than one line big
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		MinMaxAttribute minMax = attribute as MinMaxAttribute;

		// by default just return the standard line height
		float size = EditorGUIUtility.singleLineHeight * 3;
        
		// if we have a special mode, add two extra lines!
		if (minMax.ShowEditRange || minMax.ShowDebugValues)
		{
			size += EditorGUIUtility.singleLineHeight * 2;
		}

		return size;
	}
}

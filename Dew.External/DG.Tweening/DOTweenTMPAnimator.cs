using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace DG.Tweening;

public class DOTweenTMPAnimator : IDisposable
{
	private struct CharVertices
	{
		public Vector3 bottomLeft;

		public Vector3 topLeft;

		public Vector3 topRight;

		public Vector3 bottomRight;

		public CharVertices(Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight)
		{
			this.bottomLeft = bottomLeft;
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomRight = bottomRight;
		}
	}

	private struct CharTransform
	{
		public int charIndex;

		public Vector3 offset;

		public Quaternion rotation;

		public Vector3 scale;

		private Vector3 _topLeftShift;

		private Vector3 _topRightShift;

		private Vector3 _bottomLeftShift;

		private Vector3 _bottomRightShift;

		private Vector3 _charMidBaselineOffset;

		private int _matIndex;

		private int _firstVertexIndex;

		private TMP_MeshInfo _meshInfo;

		public bool isVisible { get; private set; }

		public CharTransform(int charIndex, TMP_TextInfo textInfo, TMP_MeshInfo[] cachedMeshInfos)
		{
			this = default(CharTransform);
			this.charIndex = charIndex;
			offset = Vector3.zero;
			rotation = Quaternion.identity;
			scale = Vector3.one;
			Refresh(textInfo, cachedMeshInfos);
		}

		public void Refresh(TMP_TextInfo textInfo, TMP_MeshInfo[] cachedMeshInfos)
		{
			TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
			bool isSpaceChar = charInfo.character == ' ';
			isVisible = charInfo.isVisible && !isSpaceChar;
			_matIndex = charInfo.materialReferenceIndex;
			_firstVertexIndex = charInfo.vertexIndex;
			_meshInfo = textInfo.meshInfo[_matIndex];
			Vector3[] cachedVertices = cachedMeshInfos[_matIndex].vertices;
			_charMidBaselineOffset = (isSpaceChar ? Vector3.zero : ((cachedVertices[_firstVertexIndex] + cachedVertices[_firstVertexIndex + 2]) * 0.5f));
		}

		public void ResetAll(TMP_Text target, TMP_MeshInfo[] meshInfos, TMP_MeshInfo[] cachedMeshInfos)
		{
			ResetGeometry(target, cachedMeshInfos);
			ResetColors(target, meshInfos);
		}

		public void ResetTransformationData()
		{
			offset = Vector3.zero;
			rotation = Quaternion.identity;
			scale = Vector3.one;
			_topLeftShift = (_topRightShift = (_bottomLeftShift = (_bottomRightShift = Vector3.zero)));
		}

		public void ResetGeometry(TMP_Text target, TMP_MeshInfo[] cachedMeshInfos)
		{
			ResetTransformationData();
			Vector3[] vertices = _meshInfo.vertices;
			Vector3[] cachedVertices = cachedMeshInfos[_matIndex].vertices;
			vertices[_firstVertexIndex] = cachedVertices[_firstVertexIndex];
			vertices[_firstVertexIndex + 1] = cachedVertices[_firstVertexIndex + 1];
			vertices[_firstVertexIndex + 2] = cachedVertices[_firstVertexIndex + 2];
			vertices[_firstVertexIndex + 3] = cachedVertices[_firstVertexIndex + 3];
			_meshInfo.mesh.vertices = _meshInfo.vertices;
			target.UpdateGeometry(_meshInfo.mesh, _matIndex);
		}

		public void ResetColors(TMP_Text target, TMP_MeshInfo[] meshInfos)
		{
			Color color = target.color;
			Color32[] colors = meshInfos[_matIndex].colors32;
			colors[_firstVertexIndex] = color;
			colors[_firstVertexIndex + 1] = color;
			colors[_firstVertexIndex + 2] = color;
			colors[_firstVertexIndex + 3] = color;
			target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
		}

		public Color32 GetColor(TMP_MeshInfo[] meshInfos)
		{
			return meshInfos[_matIndex].colors32[_firstVertexIndex];
		}

		public CharVertices GetVertices()
		{
			return new CharVertices(_meshInfo.vertices[_firstVertexIndex], _meshInfo.vertices[_firstVertexIndex + 1], _meshInfo.vertices[_firstVertexIndex + 2], _meshInfo.vertices[_firstVertexIndex + 3]);
		}

		public void UpdateAlpha(TMP_Text target, Color alphaColor, TMP_MeshInfo[] meshInfos, bool apply = true)
		{
			byte alphaByte = (byte)(alphaColor.a * 255f);
			Color32[] colors = meshInfos[_matIndex].colors32;
			colors[_firstVertexIndex].a = alphaByte;
			colors[_firstVertexIndex + 1].a = alphaByte;
			colors[_firstVertexIndex + 2].a = alphaByte;
			colors[_firstVertexIndex + 3].a = alphaByte;
			if (apply)
			{
				target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}
		}

		public void UpdateColor(TMP_Text target, Color32 color, TMP_MeshInfo[] meshInfos, bool apply = true)
		{
			Color32[] colors = meshInfos[_matIndex].colors32;
			colors[_firstVertexIndex] = color;
			colors[_firstVertexIndex + 1] = color;
			colors[_firstVertexIndex + 2] = color;
			colors[_firstVertexIndex + 3] = color;
			if (apply)
			{
				target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}
		}

		public void UpdateGeometry(TMP_Text target, Vector3 offset, Quaternion rotation, Vector3 scale, TMP_MeshInfo[] cachedMeshInfos, bool apply = true)
		{
			this.offset = offset;
			this.rotation = rotation;
			this.scale = scale;
			if (apply)
			{
				Vector3[] destinationVertices = _meshInfo.vertices;
				Vector3[] cachedVertices = cachedMeshInfos[_matIndex].vertices;
				destinationVertices[_firstVertexIndex] = cachedVertices[_firstVertexIndex] - _charMidBaselineOffset;
				destinationVertices[_firstVertexIndex + 1] = cachedVertices[_firstVertexIndex + 1] - _charMidBaselineOffset;
				destinationVertices[_firstVertexIndex + 2] = cachedVertices[_firstVertexIndex + 2] - _charMidBaselineOffset;
				destinationVertices[_firstVertexIndex + 3] = cachedVertices[_firstVertexIndex + 3] - _charMidBaselineOffset;
				Matrix4x4 matrix = Matrix4x4.TRS(this.offset, this.rotation, this.scale);
				destinationVertices[_firstVertexIndex] = matrix.MultiplyPoint3x4(destinationVertices[_firstVertexIndex]) + _charMidBaselineOffset + _bottomLeftShift;
				destinationVertices[_firstVertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[_firstVertexIndex + 1]) + _charMidBaselineOffset + _topLeftShift;
				destinationVertices[_firstVertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[_firstVertexIndex + 2]) + _charMidBaselineOffset + _topRightShift;
				destinationVertices[_firstVertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[_firstVertexIndex + 3]) + _charMidBaselineOffset + _bottomRightShift;
				_meshInfo.mesh.vertices = _meshInfo.vertices;
				target.UpdateGeometry(_meshInfo.mesh, _matIndex);
			}
		}

		public void ShiftVertices(TMP_Text target, Vector3 topLeftShift, Vector3 topRightShift, Vector3 bottomLeftShift, Vector3 bottomRightShift)
		{
			_topLeftShift += topLeftShift;
			_topRightShift += topRightShift;
			_bottomLeftShift += bottomLeftShift;
			_bottomRightShift += bottomRightShift;
			Vector3[] destinationVertices = _meshInfo.vertices;
			destinationVertices[_firstVertexIndex] += _bottomLeftShift;
			destinationVertices[_firstVertexIndex + 1] = destinationVertices[_firstVertexIndex + 1] + _topLeftShift;
			destinationVertices[_firstVertexIndex + 2] = destinationVertices[_firstVertexIndex + 2] + _topRightShift;
			destinationVertices[_firstVertexIndex + 3] = destinationVertices[_firstVertexIndex + 3] + _bottomRightShift;
			_meshInfo.mesh.vertices = _meshInfo.vertices;
			target.UpdateGeometry(_meshInfo.mesh, _matIndex);
		}

		public void ResetVerticesShift(TMP_Text target)
		{
			Vector3[] destinationVertices = _meshInfo.vertices;
			destinationVertices[_firstVertexIndex] -= _bottomLeftShift;
			destinationVertices[_firstVertexIndex + 1] = destinationVertices[_firstVertexIndex + 1] - _topLeftShift;
			destinationVertices[_firstVertexIndex + 2] = destinationVertices[_firstVertexIndex + 2] - _topRightShift;
			destinationVertices[_firstVertexIndex + 3] = destinationVertices[_firstVertexIndex + 3] - _bottomRightShift;
			_meshInfo.mesh.vertices = _meshInfo.vertices;
			target.UpdateGeometry(_meshInfo.mesh, _matIndex);
			_topLeftShift = (_topRightShift = (_bottomLeftShift = (_bottomRightShift = Vector3.zero)));
		}
	}

	private static readonly Dictionary<TMP_Text, DOTweenTMPAnimator> _targetToAnimator = new Dictionary<TMP_Text, DOTweenTMPAnimator>();

	private readonly List<CharTransform> _charTransforms = new List<CharTransform>();

	private TMP_MeshInfo[] _cachedMeshInfos;

	private bool _ignoreTextChangedEvent;

	public TMP_Text target { get; private set; }

	public TMP_TextInfo textInfo { get; private set; }

	public DOTweenTMPAnimator(TMP_Text target)
	{
		if (target == null)
		{
			Debugger.LogError("DOTweenTMPAnimator target can't be null");
			return;
		}
		if (!target.gameObject.activeInHierarchy)
		{
			Debugger.LogError("You can't create a DOTweenTMPAnimator if its target is disabled");
			return;
		}
		if (_targetToAnimator.ContainsKey(target))
		{
			if (Debugger.logPriority >= 2)
			{
				Debugger.Log($"A DOTweenTMPAnimator for \"{target}\" already exists: disposing it because you can't have more than one DOTweenTMPAnimator for the same TextMesh Pro object. If you have tweens running on the disposed DOTweenTMPAnimator you should kill them manually");
			}
			_targetToAnimator[target].Dispose();
			_targetToAnimator.Remove(target);
		}
		this.target = target;
		_targetToAnimator.Add(target, this);
		Refresh();
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
	}

	public static void DisposeInstanceFor(TMP_Text target)
	{
		if (_targetToAnimator.ContainsKey(target))
		{
			_targetToAnimator[target].Dispose();
			_targetToAnimator.Remove(target);
		}
	}

	public void Dispose()
	{
		target = null;
		_charTransforms.Clear();
		textInfo = null;
		_cachedMeshInfos = null;
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
	}

	public void Refresh()
	{
		_ignoreTextChangedEvent = true;
		target.ForceMeshUpdate(ignoreActiveState: true);
		textInfo = target.textInfo;
		_cachedMeshInfos = textInfo.CopyMeshInfoVertexData();
		int totChars = textInfo.characterCount;
		int totCurrent = _charTransforms.Count;
		if (totCurrent > totChars)
		{
			_charTransforms.RemoveRange(totChars, totCurrent - totChars);
			totCurrent = totChars;
		}
		for (int i = 0; i < totCurrent; i++)
		{
			CharTransform c = _charTransforms[i];
			c.ResetTransformationData();
			c.Refresh(textInfo, _cachedMeshInfos);
			_charTransforms[i] = c;
		}
		for (int j = totCurrent; j < totChars; j++)
		{
			_charTransforms.Add(new CharTransform(j, textInfo, _cachedMeshInfos));
		}
		_ignoreTextChangedEvent = false;
	}

	public void Reset()
	{
		int totCurrent = _charTransforms.Count;
		for (int i = 0; i < totCurrent; i++)
		{
			_charTransforms[i].ResetAll(target, textInfo.meshInfo, _cachedMeshInfos);
		}
	}

	private void OnTextChanged(global::UnityEngine.Object obj)
	{
		if (!_ignoreTextChangedEvent && !(target == null) && !(obj != target))
		{
			Refresh();
		}
	}

	private bool ValidateChar(int charIndex, bool isTween = true)
	{
		if (textInfo.characterCount <= charIndex)
		{
			Debugger.LogError($"CharIndex {charIndex} doesn't exist");
			return false;
		}
		if (!textInfo.characterInfo[charIndex].isVisible)
		{
			if (Debugger.logPriority > 1)
			{
				if (isTween)
				{
					Debugger.Log($"CharIndex {charIndex} isn't visible, ignoring it and returning an empty tween (TextMesh Pro will behave weirdly if invisible chars are included in the animation)");
				}
				else
				{
					Debugger.Log($"CharIndex {charIndex} isn't visible, ignoring it");
				}
			}
			return false;
		}
		return true;
	}

	private bool ValidateSpan(int fromCharIndex, int toCharIndex, out int firstVisibleCharIndex, out int lastVisibleCharIndex)
	{
		firstVisibleCharIndex = -1;
		lastVisibleCharIndex = -1;
		int charCount = textInfo.characterCount;
		if (fromCharIndex >= charCount)
		{
			return false;
		}
		if (toCharIndex >= charCount)
		{
			toCharIndex = charCount - 1;
		}
		for (int i = fromCharIndex; i < toCharIndex + 1; i++)
		{
			if (_charTransforms[i].isVisible)
			{
				firstVisibleCharIndex = i;
				break;
			}
		}
		if (firstVisibleCharIndex == -1)
		{
			return false;
		}
		for (int i2 = toCharIndex; i2 > firstVisibleCharIndex - 1; i2--)
		{
			if (_charTransforms[i2].isVisible)
			{
				lastVisibleCharIndex = i2;
				break;
			}
		}
		if (lastVisibleCharIndex == -1)
		{
			return false;
		}
		return true;
	}

	public void SkewSpanX(int fromCharIndex, int toCharIndex, float skewFactor, bool skewTop = true)
	{
		if (!ValidateSpan(fromCharIndex, toCharIndex, out var firstVisibleCharIndex, out var lastVisibleCharIndex))
		{
			return;
		}
		for (int i = firstVisibleCharIndex; i < lastVisibleCharIndex + 1; i++)
		{
			if (_charTransforms[i].isVisible)
			{
				_charTransforms[i].GetVertices();
				SkewCharX(i, skewFactor, skewTop);
			}
		}
	}

	public void SkewSpanY(int fromCharIndex, int toCharIndex, float skewFactor, TMPSkewSpanMode mode = TMPSkewSpanMode.Default, bool skewRight = true)
	{
		if (!ValidateSpan(fromCharIndex, toCharIndex, out var firstVisibleCharIndex, out var lastVisibleCharIndex))
		{
			return;
		}
		if (mode == TMPSkewSpanMode.AsMaxSkewFactor)
		{
			CharVertices firstVisibleCharVertices = _charTransforms[firstVisibleCharIndex].GetVertices();
			CharVertices lastVisibleCharVertices = _charTransforms[lastVisibleCharIndex].GetVertices();
			float spanW = Mathf.Abs(lastVisibleCharVertices.bottomRight.x - firstVisibleCharVertices.bottomLeft.x);
			float ratio = Mathf.Abs(lastVisibleCharVertices.topRight.y - lastVisibleCharVertices.bottomRight.y) / spanW;
			skewFactor *= ratio;
		}
		float offsetY = 0f;
		CharVertices prevCharVertices = default(CharVertices);
		float prevCharSkew = 0f;
		if (skewRight)
		{
			for (int i = firstVisibleCharIndex; i < lastVisibleCharIndex + 1; i++)
			{
				if (_charTransforms[i].isVisible)
				{
					CharVertices v = _charTransforms[i].GetVertices();
					float num = SkewCharY(i, skewFactor, skewRight);
					if (i > firstVisibleCharIndex)
					{
						float prevCharW = Mathf.Abs(prevCharVertices.bottomLeft.x - prevCharVertices.bottomRight.x);
						float charsDist = Mathf.Abs(v.bottomLeft.x - prevCharVertices.bottomRight.x);
						offsetY += prevCharSkew + prevCharSkew * charsDist / prevCharW;
						SetCharOffset(i, new Vector3(0f, _charTransforms[i].offset.y + offsetY, 0f));
					}
					prevCharVertices = v;
					prevCharSkew = num;
				}
			}
			return;
		}
		for (int i2 = lastVisibleCharIndex; i2 > firstVisibleCharIndex - 1; i2--)
		{
			if (_charTransforms[i2].isVisible)
			{
				CharVertices v2 = _charTransforms[i2].GetVertices();
				float num2 = SkewCharY(i2, skewFactor, skewRight);
				if (i2 < lastVisibleCharIndex)
				{
					float prevCharW2 = Mathf.Abs(prevCharVertices.bottomLeft.x - prevCharVertices.bottomRight.x);
					float charsDist2 = Mathf.Abs(v2.bottomRight.x - prevCharVertices.bottomLeft.x);
					offsetY += prevCharSkew + prevCharSkew * charsDist2 / prevCharW2;
					SetCharOffset(i2, new Vector3(0f, _charTransforms[i2].offset.y + offsetY, 0f));
				}
				prevCharVertices = v2;
				prevCharSkew = num2;
			}
		}
	}

	public Color GetCharColor(int charIndex)
	{
		if (!ValidateChar(charIndex))
		{
			return Color.white;
		}
		return _charTransforms[charIndex].GetColor(textInfo.meshInfo);
	}

	public Vector3 GetCharOffset(int charIndex)
	{
		if (!ValidateChar(charIndex))
		{
			return Vector3.zero;
		}
		return _charTransforms[charIndex].offset;
	}

	public Vector3 GetCharRotation(int charIndex)
	{
		if (!ValidateChar(charIndex))
		{
			return Vector3.zero;
		}
		return _charTransforms[charIndex].rotation.eulerAngles;
	}

	public Vector3 GetCharScale(int charIndex)
	{
		if (!ValidateChar(charIndex))
		{
			return Vector3.zero;
		}
		return _charTransforms[charIndex].scale;
	}

	public void SetCharColor(int charIndex, Color32 color)
	{
		if (ValidateChar(charIndex))
		{
			CharTransform c = _charTransforms[charIndex];
			c.UpdateColor(target, color, textInfo.meshInfo);
			_charTransforms[charIndex] = c;
		}
	}

	public void SetCharOffset(int charIndex, Vector3 offset)
	{
		if (ValidateChar(charIndex))
		{
			CharTransform c = _charTransforms[charIndex];
			c.UpdateGeometry(target, offset, c.rotation, c.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = c;
		}
	}

	public void SetCharRotation(int charIndex, Vector3 rotation)
	{
		if (ValidateChar(charIndex))
		{
			CharTransform c = _charTransforms[charIndex];
			c.UpdateGeometry(target, c.offset, Quaternion.Euler(rotation), c.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = c;
		}
	}

	public void SetCharScale(int charIndex, Vector3 scale)
	{
		if (ValidateChar(charIndex))
		{
			CharTransform c = _charTransforms[charIndex];
			c.UpdateGeometry(target, c.offset, c.rotation, scale, _cachedMeshInfos);
			_charTransforms[charIndex] = c;
		}
	}

	public void ShiftCharVertices(int charIndex, Vector3 topLeftShift, Vector3 topRightShift, Vector3 bottomLeftShift, Vector3 bottomRightShift)
	{
		if (ValidateChar(charIndex))
		{
			CharTransform c = _charTransforms[charIndex];
			c.ShiftVertices(target, topLeftShift, topRightShift, bottomLeftShift, bottomRightShift);
			_charTransforms[charIndex] = c;
		}
	}

	public float SkewCharX(int charIndex, float skewFactor, bool skewTop = true)
	{
		if (!ValidateChar(charIndex))
		{
			return 0f;
		}
		Vector3 skewV = new Vector3(skewFactor, 0f, 0f);
		CharTransform c = _charTransforms[charIndex];
		if (skewTop)
		{
			c.ShiftVertices(target, skewV, skewV, Vector3.zero, Vector3.zero);
		}
		else
		{
			c.ShiftVertices(target, Vector3.zero, Vector3.zero, skewV, skewV);
		}
		_charTransforms[charIndex] = c;
		return skewFactor;
	}

	public float SkewCharY(int charIndex, float skewFactor, bool skewRight = true, bool fixedSkew = false)
	{
		if (!ValidateChar(charIndex))
		{
			return 0f;
		}
		float skew = (fixedSkew ? skewFactor : (skewFactor * textInfo.characterInfo[charIndex].aspectRatio));
		Vector3 skewV = new Vector3(0f, skew, 0f);
		CharTransform c = _charTransforms[charIndex];
		if (skewRight)
		{
			c.ShiftVertices(target, Vector3.zero, skewV, Vector3.zero, skewV);
		}
		else
		{
			c.ShiftVertices(target, skewV, Vector3.zero, skewV, Vector3.zero);
		}
		_charTransforms[charIndex] = c;
		return skew;
	}

	public void ResetVerticesShift(int charIndex)
	{
		if (ValidateChar(charIndex))
		{
			CharTransform c = _charTransforms[charIndex];
			c.ResetVerticesShift(target);
			_charTransforms[charIndex] = c;
		}
	}

	public TweenerCore<Color, Color, ColorOptions> DOFadeChar(int charIndex, float endValue, float duration)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		return DOTween.ToAlpha(() => _charTransforms[charIndex].GetColor(textInfo.meshInfo), delegate(Color x)
		{
			_charTransforms[charIndex].UpdateAlpha(target, x, textInfo.meshInfo);
		}, endValue, duration);
	}

	public TweenerCore<Color, Color, ColorOptions> DOColorChar(int charIndex, Color endValue, float duration)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		return DOTween.To(() => _charTransforms[charIndex].GetColor(textInfo.meshInfo), delegate(Color x)
		{
			_charTransforms[charIndex].UpdateColor(target, x, textInfo.meshInfo);
		}, endValue, duration);
	}

	public TweenerCore<Vector3, Vector3, VectorOptions> DOOffsetChar(int charIndex, Vector3 endValue, float duration)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		return DOTween.To(() => _charTransforms[charIndex].offset, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, x, value.rotation, value.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, endValue, duration);
	}

	public TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotateChar(int charIndex, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => _charTransforms[charIndex].rotation, delegate(Quaternion x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, value.offset, x, value.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, endValue, duration);
		tweenerCore.plugOptions.rotateMode = mode;
		return tweenerCore;
	}

	public TweenerCore<Vector3, Vector3, VectorOptions> DOScaleChar(int charIndex, float endValue, float duration)
	{
		return DOScaleChar(charIndex, new Vector3(endValue, endValue, endValue), duration);
	}

	public TweenerCore<Vector3, Vector3, VectorOptions> DOScaleChar(int charIndex, Vector3 endValue, float duration)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		return DOTween.To(() => _charTransforms[charIndex].scale, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, value.offset, value.rotation, x, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, endValue, duration);
	}

	public Tweener DOPunchCharOffset(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Punch(() => _charTransforms[charIndex].offset, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, x, value.rotation, value.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, punch, duration, vibrato, elasticity);
	}

	public Tweener DOPunchCharRotation(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Punch(() => _charTransforms[charIndex].rotation.eulerAngles, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, value.offset, Quaternion.Euler(x), value.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, punch, duration, vibrato, elasticity);
	}

	public Tweener DOPunchCharScale(int charIndex, float punch, float duration, int vibrato = 10, float elasticity = 1f)
	{
		return DOPunchCharScale(charIndex, new Vector3(punch, punch, punch), duration, vibrato, elasticity);
	}

	public Tweener DOPunchCharScale(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Punch(() => _charTransforms[charIndex].scale, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, value.offset, value.rotation, x, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, punch, duration, vibrato, elasticity);
	}

	public Tweener DOShakeCharOffset(int charIndex, float duration, float strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		return DOShakeCharOffset(charIndex, duration, new Vector3(strength, strength, strength), vibrato, randomness, fadeOut);
	}

	public Tweener DOShakeCharOffset(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => _charTransforms[charIndex].offset, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, x, value.rotation, value.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, duration, strength, vibrato, randomness, fadeOut);
	}

	public Tweener DOShakeCharRotation(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => _charTransforms[charIndex].rotation.eulerAngles, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, value.offset, Quaternion.Euler(x), value.scale, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, duration, strength, vibrato, randomness, fadeOut);
	}

	public Tweener DOShakeCharScale(int charIndex, float duration, float strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		return DOShakeCharScale(charIndex, duration, new Vector3(strength, strength, strength), vibrato, randomness, fadeOut);
	}

	public Tweener DOShakeCharScale(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (!ValidateChar(charIndex))
		{
			return null;
		}
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => _charTransforms[charIndex].scale, delegate(Vector3 x)
		{
			CharTransform value = _charTransforms[charIndex];
			value.UpdateGeometry(target, value.offset, value.rotation, x, _cachedMeshInfos);
			_charTransforms[charIndex] = value;
		}, duration, strength, vibrato, randomness, fadeOut);
	}
}

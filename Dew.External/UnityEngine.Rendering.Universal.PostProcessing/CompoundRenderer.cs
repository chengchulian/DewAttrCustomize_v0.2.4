using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal.PostProcessing;

public abstract class CompoundRenderer : IDisposable
{
	private static class ShaderConstants
	{
		public static readonly int _SourceSize = Shader.PropertyToID("_SourceSize");
	}

	private bool _initialized;

	protected GraphicsFormat _defaultHDRFormat;

	protected bool _useRGBM;

	public virtual bool visibleInSceneView => true;

	public virtual ScriptableRenderPassInput input => ScriptableRenderPassInput.Color;

	public bool Initialized => _initialized;

	internal void InitializeInternal()
	{
		Initialize();
		_initialized = true;
	}

	public virtual void Initialize()
	{
		if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
		{
			_defaultHDRFormat = GraphicsFormat.B10G11R11_UFloatPack32;
			_useRGBM = false;
		}
		else
		{
			_defaultHDRFormat = ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? GraphicsFormat.R8G8B8A8_SRGB : GraphicsFormat.R8G8B8A8_UNorm);
			_useRGBM = true;
		}
	}

	public virtual bool Setup(in RenderingData renderingData, InjectionPoint injectionPoint)
	{
		return true;
	}

	public abstract void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, InjectionPoint injectionPoint);

	public virtual void Dispose(bool disposing)
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public static RenderTextureDescriptor GetTempRTDescriptor(in RenderingData renderingData)
	{
		RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
		descriptor.depthBufferBits = 0;
		descriptor.msaaSamples = 1;
		return descriptor;
	}

	public static RenderTextureDescriptor GetTempRTDescriptor(in RenderingData renderingData, int width, int height, GraphicsFormat format)
	{
		if (width <= 0 || height <= 0)
		{
			Debug.LogError($"Invalid parameters for GetTempRTDescriptor: {width}, {height}.");
		}
		RenderTextureDescriptor descriptor = GetTempRTDescriptor(in renderingData);
		descriptor.width = width;
		descriptor.height = height;
		return descriptor;
	}

	public static void SetSourceSize(CommandBuffer cmd, RenderTextureDescriptor desc)
	{
		float width = desc.width;
		float height = desc.height;
		if (desc.useDynamicScale)
		{
			width *= ScalableBufferManager.widthScaleFactor;
			height *= ScalableBufferManager.heightScaleFactor;
		}
		cmd.SetGlobalVector(ShaderConstants._SourceSize, new Vector4(width, height, 1f / width, 1f / height));
	}
}

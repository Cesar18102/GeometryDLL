﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GlTexture
{
	private const int TextureCount = 1;

	private IEnumerable<int> Textures
	{
		get;
		set;
	}

	private Bitmap textureIMG
	{
		get;
		set;
	}

	private BitmapData bitmapData
	{
		get;
		set;
	}

	public GlTexture(Bitmap bmp)
	{
	}

	public GlTexture(string filename)
	{
	}

	public virtual void BindTexture()
	{
		throw new System.NotImplementedException();
	}

	public virtual void UnbindTexture()
	{
		throw new System.NotImplementedException();
	}

}


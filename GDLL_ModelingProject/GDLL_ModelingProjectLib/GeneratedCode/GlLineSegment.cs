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

public class GlLineSegment : GlVectorR2
{
	private const float FAULT = 0.07f;

	private GlPointR2 startPoint
	{
		get;
		set;
	}

	private GlPointR2 endPoint
	{
		get;
		set;
	}

	public virtual GlPointR2 EndPoint
	{
		get

		{
			throw new System.NotImplementedException();
		}
		set

		{
			throw new System.NotImplementedException();
		}
	}

	public virtual GlPointR2 StartPoint
	{
		get

		{
			throw new System.NotImplementedException();
		}
		set

		{
			throw new System.NotImplementedException();
		}
	}

	public virtual void Draw()
	{
		throw new System.NotImplementedException();
	}

	public virtual bool isPointBelongs(GlPointR2 P)
	{
		throw new System.NotImplementedException();
	}

	public GlLineSegment(GlPointR2 startPoint, GlPointR2 endPoint)
	{
	}

}


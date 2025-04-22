using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI;

public class UITooltipLines
{
	public enum LineStyle
	{
		Default,
		Title,
		Description,
		Custom
	}

	[Serializable]
	public class Line
	{
		public string left;

		public string right;

		public bool isComplete;

		public RectOffset padding;

		public LineStyle style;

		public string customStyle;

		public Line(string left, string right, bool isComplete, RectOffset padding, LineStyle style, string customStyle)
		{
			this.left = left;
			this.right = right;
			this.isComplete = isComplete;
			this.padding = padding;
			this.style = style;
			this.customStyle = customStyle;
		}
	}

	public class Lines : List<Line>
	{
	}

	public Lines lineList = new Lines();

	public void AddLine(string leftContent, string rightContent)
	{
		lineList.Add(new Line(leftContent, rightContent, isComplete: true, new RectOffset(), LineStyle.Default, ""));
	}

	public void AddLine(string leftContent, string rightContent, RectOffset padding)
	{
		lineList.Add(new Line(leftContent, rightContent, isComplete: true, padding, LineStyle.Default, ""));
	}

	public void AddLine(string content)
	{
		lineList.Add(new Line(content, string.Empty, isComplete: true, new RectOffset(), LineStyle.Default, ""));
	}

	public void AddLine(string content, RectOffset padding)
	{
		lineList.Add(new Line(content, string.Empty, isComplete: true, padding, LineStyle.Default, ""));
	}

	public void AddLine(string content, RectOffset padding, LineStyle style)
	{
		lineList.Add(new Line(content, string.Empty, isComplete: true, padding, style, ""));
	}

	public void AddLine(string content, RectOffset padding, string customStyle)
	{
		lineList.Add(new Line(content, string.Empty, isComplete: true, padding, LineStyle.Custom, customStyle));
	}

	public void AddLine(string leftContent, string rightContent, RectOffset padding, LineStyle style)
	{
		lineList.Add(new Line(leftContent, rightContent, isComplete: true, padding, style, ""));
	}

	public void AddLine(string leftContent, string rightContent, RectOffset padding, string customStyle)
	{
		lineList.Add(new Line(leftContent, rightContent, isComplete: true, padding, LineStyle.Custom, customStyle));
	}

	public void AddColumn(string content)
	{
		AddColumn(content, LineStyle.Default, "");
	}

	public void AddColumn(string content, LineStyle style)
	{
		AddColumn(content, style, "");
	}

	public void AddColumn(string content, string customStyle)
	{
		AddColumn(content, LineStyle.Custom, customStyle);
	}

	public void AddColumn(string content, LineStyle style, string customStyle)
	{
		if (lineList.Count == 0)
		{
			lineList.Add(new Line(content, string.Empty, isComplete: false, new RectOffset(), style, customStyle));
			return;
		}
		Line line = lineList[lineList.Count - 1];
		if (!line.isComplete)
		{
			line.right = content;
			line.isComplete = true;
		}
		else
		{
			lineList.Add(new Line(content, string.Empty, isComplete: false, new RectOffset(), style, customStyle));
		}
	}
}

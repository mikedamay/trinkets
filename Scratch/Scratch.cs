using System;

public class Scratch
{
	public static void Main()
	{
		var a = new At(typeof(Scratch), "somePath");
		var b = new At("someotherPath");
		(Type t, string s) = a;
		String s2 = b;
	}
}
public class At
{
	Type _t;
	string _s;
	public At(Type t, string s)
	{
		_t = t;
		_s = s;
	}
	public At(string s)
	{
		_s = s;
	}
	public void Deconstruct(out Type t, out string s)
	{
		t = _t;
		s = _s;
	}
	public static implicit operator string (At at)
	{
		return at._s;
	}
}

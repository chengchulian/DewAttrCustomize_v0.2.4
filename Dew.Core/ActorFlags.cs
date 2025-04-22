using System;
using System.Collections.Generic;

internal struct ActorFlags
{
	private List<Type> _flags;

	public ActorFlags ShallowCopy()
	{
		ActorFlags result = default(ActorFlags);
		result._flags = _flags;
		return result;
	}

	public ActorFlags DeepCopy()
	{
		if (_flags == null)
		{
			return default(ActorFlags);
		}
		ActorFlags result = default(ActorFlags);
		result._flags = new List<Type>(_flags);
		return result;
	}

	public void Add(Actor actor)
	{
		Add(actor.GetType());
	}

	public void Add(Type actorType)
	{
		if (_flags == null)
		{
			_flags = new List<Type>(4);
		}
		_flags.Add(actorType);
	}

	public bool Contains(Actor actor)
	{
		return Contains(actor.GetType());
	}

	public bool Contains(Type actorType)
	{
		if (_flags == null)
		{
			return false;
		}
		return _flags.Contains(actorType);
	}
}

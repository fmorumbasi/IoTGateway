﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Waher.Things;

namespace Waher.Networking.XMPP.Provisioning
{
	/// <summary>
	/// Event arguments class for search result callback methods.
	/// </summary>
	public class SearchResultEventArgs : IqResultEventArgs
	{
		private SearchResultThing[] things;
		private int offset;
		private int maxCount;
		private bool more;

		internal SearchResultEventArgs(IqResultEventArgs e, object State, int Offset, int MaxCount, bool More, SearchResultThing[] Things)
			: base(e)
		{
			this.State = State;
			this.offset = Offset;
			this.maxCount = MaxCount;
			this.more = More;
			this.things = Things;
		}

		/// <summary>
		/// Offset used in search.
		/// </summary>
		public int Offset
		{
			get { return this.offset; }
		}

		/// <summary>
		/// Max count used in search.
		/// </summary>
		public int MaxCount
		{
			get { return this.maxCount; }
		}

		/// <summary>
		/// If there are more results available on the server.
		/// </summary>
		public bool More
		{
			get { return this.more; }
		}

		/// <summary>
		/// Things returned in the search result.
		/// </summary>
		public SearchResultThing[] Things
		{
			get { return this.things; }
		}

	}
}

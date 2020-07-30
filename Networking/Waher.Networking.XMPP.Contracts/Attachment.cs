﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Waher.Networking.XMPP.Contracts
{
	/// <summary>
	/// Contains a reference to an attachment assigned to a legal object.
	/// </summary>
	public class Attachment
	{
		private string id = null;
		private string contentType = string.Empty;
		private string fileName = null;
		private string url = null;
		private byte[] signature = null;
		private DateTime timestamp = DateTime.MinValue;

		/// <summary>
		/// Contains a reference to an attachment assigned to a legal object.
		/// </summary>
		public Attachment()
		{
		}

		/// <summary>
		/// Attachment ID
		/// </summary>
		public string Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		/// <summary>
		/// Internet Content Type of binary attachment.
		/// </summary>
		public string ContentType
		{
			get => this.contentType;
			set => this.contentType = value;
		}

		/// <summary>
		/// Filename of attachment.
		/// </summary>
		public string FileName
		{
			get => this.fileName;
			set => this.fileName = value;
		}

		/// <summary>
		/// URL to retrieve attachment, if provided.
		/// </summary>
		public string Url
		{
			get => this.url;
			set => this.url = value;
		}

		/// <summary>
		/// Binary signature of the attachment, generated by an approved legal identity of the account-holder.
		/// If no signature, attribute is null.
		/// </summary>
		public byte[] Signature
		{
			get => this.signature;
			set => this.signature = value;
		}

		/// <summary>
		/// Timestamp of signature. If no signature, value is <see cref="DateTime.MinValue"/>
		/// </summary>
		public DateTime Timestamp
		{
			get => this.timestamp;
			set => this.timestamp = value;
		}
	}
}

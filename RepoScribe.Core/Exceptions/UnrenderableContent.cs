using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoScribe.Core.Exceptions
{
    class UnrenderableContent : Exception
	{
		public UnrenderableContent(string message) : base(message)
	}
}

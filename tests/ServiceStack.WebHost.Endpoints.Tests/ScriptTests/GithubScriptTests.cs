using NUnit.Framework;
using ServiceStack.Script;
using ServiceStack.Text;

namespace ServiceStack.WebHost.Endpoints.Tests.ScriptTests
{
    [Ignore("Integration Tests")]
    public class GithubScriptTests
    {
        public ScriptContext CreateScriptContext()
        {
            return new ScriptContext {
                Plugins = { new GithubPlugin() },
                ScriptMethods = { new InfoScripts() },
            };
        }
        
        [Test]
        public void Can_write_and_read_gist_files()
        {
            var context = CreateScriptContext().Init();

            var output = context.EvaluateScript(@"
{{ 'GITHUB_GIST_TOKEN' | envVariable | githubGateway | assignTo: gateway }}
{{ gateway.githubCreateGist('Hello World Examples', {
     'hello_world_ruby.txt': 'Run `ruby hello_world.rb` to print Hello World',
     'hello_world_python.txt': 'Run `python hello_world.py` to print Hello World',
   })
   | assignTo: newGist }}

{{ { ...newGist, Files: null, Owner: null } | textDump({ caption: 'new gist' }) }}
{{ newGist.Owner | textDump({ caption: 'new gist owner' }) }}
{{ newGist.Files | textDump({ caption: 'new gist files' }) }}

{{ gateway.githubGist(newGist.Id) | assignTo: gist }}

{{ { ...gist, Files: null, Owner: null } | textDump({ caption: 'gist' }) }}
{{ gist.Files | textDump({ caption: 'gist files' }) }}
");
 
            output.Print();
        }
    }
}
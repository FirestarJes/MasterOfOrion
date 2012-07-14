using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MasterOfCentauri.Managers
{
    class ContentController
    {
        GraphicsDevice _graph;
        ContentManager _content;
        Dictionary<string, object> _contentDic;
        


        public ContentController(GraphicsDevice graph, ContentManager content)
        {
            _graph = graph;
            _content = content;
            _contentDic = new Dictionary<string, object>(); 
        }

        public T GetContent<T>(string contentName)
        {
            if (_contentDic.ContainsKey(contentName))
            {
                return (T)_contentDic[contentName];
            }
            else
            {
                T cont = _content.Load<T>(contentName);
                _contentDic.Add(contentName, cont);
                return (T)_contentDic[contentName];
            }
        }

    }
}

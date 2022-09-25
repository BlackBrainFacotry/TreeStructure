using AspNetCoreWebApi.Data;
using AspNetCoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;
using System.Linq;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly TreeStructureDbContext _context;

        public NodesController(TreeStructureDbContext context)
        {
            _context = context;
        }

        // GET: api/nodes
        [HttpGet]
        public async Task<List<Node>> GetNodesASC()
        {
            return GetTreeSortedAsc(true); 
        }

        // GET: api/nodes/{asc}
        [HttpGet("{asc}")]
        public async Task<List<Node>> GetNodes(bool asc)
        {
            return GetTreeSortedAsc(asc);
        }

        // POST: api/Posts/update
        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateNode(Node nodeToUpdate)
        {
            Node oldNode = await _context.Nodes.FindAsync(nodeToUpdate.Id);
            try
            {
                oldNode.Name = nodeToUpdate.Name;
            }
            catch (Exception ex)
            {
                return false;
            }
            
            return await _context.SaveChangesAsync() >= 1;
        }

        // POST: api/Posts/add
        [HttpPost]
        [Route("add")]
        public async Task<bool> AddNode(CreateNode nodeToCreate)
        {
            if (_context.Nodes == null)
            {
                return false;
            }
            if(nodeToCreate.ParentId == 0)
            {
                nodeToCreate.ParentId = null;
            }

            Node node = new Node();
            node.Name = nodeToCreate.Name;
            node.ParentId = nodeToCreate.ParentId;
            _context.Nodes.Add(node);

            return await _context.SaveChangesAsync() >= 1;
        }

       // POST: api/Posts/move
       [HttpPost]
       [Route("move/{id}")]
        public async Task<bool> Move(int id, Node targetNode)
        {
            if (_context.Nodes == null)
            {
                return false;
            }
            Node movedNode = await _context.Nodes.FindAsync(id);

            if (targetNode == null || targetNode.Id == 0)
            {
                movedNode.ParentId = null;
            }
            else
            {
                 Node newParentNode = await _context.Nodes.FindAsync(targetNode.Id) ;
                 movedNode.ParentId = newParentNode.Id;
            }

            return await _context.SaveChangesAsync() >= 1;
        }

        // GET: api/Posts/deletenode
        [HttpGet]
        [Route("deletenode/{id}")]
        public async Task<IActionResult> DeleteOneNode(int id)
        {
            if (_context.Nodes == null)
            {
                return NotFound();
            }
            var dismissedNode = await _context.Nodes.FindAsync(id);

            if (dismissedNode == null)
            {
                return NotFound();
            }
            await _context.Nodes.Where(e => e.ParentId == dismissedNode.Id).ForEachAsync(e => e.ParentId = dismissedNode.ParentId);  
            _context.Nodes.Remove(dismissedNode);
            await _context.SaveChangesAsync();

            return Ok(dismissedNode);
        }

        // GET: api/Posts/deleteroot
        [HttpGet]
        [Route("deleteroot/{id}")]
        public async Task<IActionResult> DeleteWithChildren(int id)
        {
            if (_context.Nodes == null)
            {
                return NotFound();
            }
            Node dismissedNode = await _context.Nodes.FindAsync(id);
            if (dismissedNode == null)
            {
                return NotFound();
            }

            List<Node> dismissedNodeList = new List<Node>();
            dismissedNodeList.Add(dismissedNode);
            dismissedNodeList.AddRange(FindAllChildrens(dismissedNode));
            _context.Nodes.RemoveRange(dismissedNodeList);
            await _context.SaveChangesAsync();

            return Ok(dismissedNodeList);
        }

        //Private
        private List<Node> allChildrens = new List<Node>();
        private void FindChildren(Node rootNode)
        {
            List<Node> childrens = new List<Node>();
            childrens.AddRange(  _context.Nodes.Where(e => rootNode.Id == e.ParentId).ToList() );
            allChildrens.AddRange(childrens);
            childrens.ForEach(parent => FindChildren(parent));            
        }
        
        private List<Node> FindAllChildrens(Node node)
        {
            allChildrens = new List<Node>();
            FindChildren(node);
            return allChildrens;
        }
        private List<Node> GetTreeSortedAsc(bool asc )
        {
            List<Node> nodeList = _context.Nodes.ToList();
            List<Node> returnList = new List<Node>();
            List<Node> parentList = new List<Node>();
            if (asc) { parentList = _context.Nodes.Where(e => e.ParentId == null).OrderBy(e => e.Name).ToList(); }
            else { parentList = _context.Nodes.Where(e => e.ParentId == null).OrderByDescending(e => e.Name).ToList(); }
            foreach (Node node in parentList)
            {
                 GenereteTree(node, nodeList, asc);
                 returnList.Add(node);
            }
            return returnList;
        }
        private void GenereteTree(Node node, List<Node> contextNodeList,bool sort)
        {
            if (node.Childrens == null)
            {
                node.Childrens = new List<Node>();
            }
            List<Node> childrenList = new List<Node>();
            if (sort)
            {
                childrenList = contextNodeList.Where(e => e.ParentId == node.Id).OrderBy(e => e.Name).ToList();
            }
            else
            {
                childrenList = contextNodeList.Where(e => e.ParentId == node.Id).OrderByDescending(e => e.Name).ToList();
            }
            if (childrenList.Count > 0)
            {
                foreach (Node item in childrenList)
                {
                    node.Childrens.Add(item);
                    GenereteTree(item, contextNodeList, sort);
                }
            }
            
            
        }
    }
}

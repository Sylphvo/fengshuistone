/* globals Tree */
'use strict';

var tree = new Tree(document.getElementById('tree'), {
  navigate: true // allow navigate with ArrowUp and ArrowDown
});

var structure = [{
  name: 'Longt 1',
  open: false,
  type: Tree.FOLDER,
  selected: false,
  children: [{
    name: 'thanbailongtu'
  }, {
    name: 'thanbailongtu2'
  }, {
    name: 'binhminh',
    type: Tree.FOLDER,
    children: [{
      name: 'hahaa',
      type: Tree.FOLDER,
      children: [{
        name: 'folder 1/1/1/1',
        type: Tree.FOLDER,
        children: [{
          name: 'file 1/1/1/1/1'
        }, {
          name: 'file 1/1/1/1/2'
        }]
      }]
    }]
  }]
}, {
  name: 'folder 2 (asynced)',
  type: Tree.FOLDER,
  asynced: true
}];
tree.json(structure);

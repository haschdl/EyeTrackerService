// P_2_1_2_03
//
// Generative Gestaltung – Creative Coding im Web
// ISBN: 978-3-87439-902-9, First Edition, Hermann Schmidt, Mainz, 2018
// Benedikt Groß, Hartmut Bohnacker, Julia Laub, Claudius Lazzeroni
// with contributions by Joey Lee and Niels Poldervaart
// Copyright 2018
//
// http://www.generative-gestaltung.de
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

/**
 * changing size of circles in a rad grid depending the mouseposition
 *
 * MOUSE
 * position x/y        : module size and offset z
 *
 * KEYS
 * s                   : save png
 */
'use strict';

//var tileCount = 200;

var moduleColor;
var moduleAlpha = .7;
var maxDistance = 500;

function setup() {
  createCanvas(windowWidth-10, windowHeight-20);
  colorMode(HSB, 1.,1.,1.,1.);
  noFill();
  strokeWeight(2);
  moduleColor = color(1,1,1, moduleAlpha);
  background(0);
  document.body.style.backgroundColor = "#000000";
}

function draw() {
   background(0);
  noStroke();


  let inc = 20;
  for (var gridY = 0; gridY < height-inc ; gridY += inc) {
    for (var gridX = 0; gridX < width-inc ; gridX += inc) {
      
      let diameter = 10;
      let c = color(pow(noise(.1 * (gridX+.2) * (gridY+.45)  ,millis()/800.),2),1.,1.,.5);
      
      if(pos && userPresent) {
          diameter = dist(pos[0]/1.25, pos[1]/1.25, gridX, gridY);
         //var diameter = dist(mouseX, mouseY, gridX, gridY);
         diameter = 40 * pow(diameter/maxDistance,.9);
      }

      push();      
      fill(c);
      translate(gridX, gridY, diameter * 5);
      rect(0, 0, diameter, diameter); // also nice: ellipse(...)
      pop();
    }
  }
}

function keyReleased() {
  if (key == 's' || key == 'S') saveCanvas(gd.timestamp(), 'png');
}


// speed of water
var speed : float = 0.7;
// transparency of water.  

var rend : Renderer;
function Update () 
{
	var theTime = Time.time;
	var moveWater = Mathf.PingPong(theTime * speed, 100) * 0.15;
	rend.GetComponent.<Renderer>().material.mainTextureOffset = Vector2( moveWater, moveWater );	

}
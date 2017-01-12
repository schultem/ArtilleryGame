using UnityEngine;
using System.Collections.Generic;

public class CylindricalTowerGenerator : MonoBehaviour {
    public GameObject brick;
    public GameObject floor;

    public int Height; //bricks
    public int Circumference; //bricks
    public float BrickOffsetAngle; //degrees
    public bool Interlock;

    private List<List<GameObject>> WallBricks = new List<List<GameObject>>();
    private List<List<GameObject>> TopBricks = new List<List<GameObject>>();

    void Start() {
        //GenerateFoundation();
        GenerateWalls();
        GenerateTop();
    }

    private void GenerateFoundation()
    {
        //tower radius from origin to the outside edge of the bricks
        //float radius = RadiusOfDiscretePolygon(Circumference, brick.transform.localScale.x) + brick.transform.localScale.z;
    }

    private void GenerateWalls()
    {
        //the pivot point for the first row of bricks above the tower origin
        Vector3 initial_offset_position = transform.position + Vector3.up * brick.transform.localScale.y / 2;

        //tower radius from origin to the center of a brick
        float radius = RadiusOfDiscretePolygon(Circumference, brick.transform.localScale.x) + brick.transform.localScale.z / 2;

        for (int row = 0; row < Height; row++)
        {
            WallBricks.Add(new List<GameObject>());
            for (int row_brick = 0; row_brick < Circumference; row_brick++)
            {
                Vector3 brick_position, brick_direction;
                Quaternion brick_position_angle, brick_look_rotation;

                //determine the angle offset and style between rows
                if (Interlock)
                    brick_position_angle = Quaternion.Euler(0, Mathf.Pow(-1, row) * BrickOffsetAngle + (row_brick * 360 / Circumference), 0);
                else
                    brick_position_angle = Quaternion.Euler(0, row * BrickOffsetAngle + (row_brick * 360 / Circumference), 0);

                //determine the brick's position around the origin of the tower
                brick_position = initial_offset_position + brick_position_angle * new Vector3(0, row * brick.transform.localScale.y, radius);

                //determine the brick rotation toward the origin of the tower
                brick_direction = (initial_offset_position + row * brick.transform.localScale.y * Vector3.up - brick_position).normalized;
                brick_look_rotation = Quaternion.LookRotation(brick_direction);

                //track the bricks by row
                WallBricks[row].Add((GameObject)Instantiate(brick, brick_position, brick_look_rotation, transform));
            }
        }
    }

    private void GenerateTop()
    {
        //the pivot point for the last row of bricks, above the tower
        Vector3 initial_offset_position = Vector3.up * (brick.transform.localScale.y / 2 + Height * brick.transform.localScale.y) + transform.position;

        //tower radius from origin to the outside edge of the bricks
        float radius = 1.2f*RadiusOfDiscretePolygon(Circumference, brick.transform.localScale.x) + brick.transform.localScale.z;

        //Create the top structural row of the tower at Height
        GameObject cap = (GameObject)Instantiate(floor, initial_offset_position, transform.localRotation, transform);
        cap.transform.localScale = new Vector3(2 * radius * cap.transform.localScale.x,
                                               cap.transform.localScale.y,
                                               2 * radius * cap.transform.localScale.z);

        //Track the rows of bricks, starting with the cap brick
        TopBricks.Add(new List<GameObject> { cap });

        //create the top decorative bricks
        List<GameObject> decorative_row = new List<GameObject>();
        for (int row_brick = 0; row_brick < (Circumference / 2); row_brick++)
        {
            Vector3 brick_position, brick_direction;
            Quaternion brick_position_angle, brick_look_rotation;

            //determine the angle offset and style between rows
            brick_position_angle = Quaternion.Euler(0, (row_brick * 360 / (Circumference / 2)), 0);

            //determine the brick's position around the origin of the tower
            brick_position = initial_offset_position + brick_position_angle * new Vector3(0, brick.transform.localScale.y, radius - brick.transform.localScale.z / 2);

            //determine the brick rotation toward the origin of the tower
            brick_direction = (initial_offset_position + brick.transform.localScale.y * Vector3.up - brick_position).normalized;
            brick_look_rotation = Quaternion.LookRotation(brick_direction);

            //track the bricks by row
            decorative_row.Add((GameObject)Instantiate(brick, brick_position, brick_look_rotation, transform));
        }
        TopBricks.Add(decorative_row);
    }

    //Calculate the distance from the center of an n-gon to a side given the number of sides and sideLength
    float RadiusOfDiscretePolygon(int sides, float sideLength)
    {
        return sideLength / (2 * Mathf.Tan(Mathf.PI / sides));
    }
}

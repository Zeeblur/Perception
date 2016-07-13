using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;

public class RS3DGameBlob : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RS3D_game_data_state_header.version_number = 1;
        RS3D_game_data_state_header.tick = 0;
        RS3D_game_data_state_header.num_rooms = 0;
        RS3D_game_data_state_header.num_occluders = 0;

        //For Testing, add one room 
        //Vector3 test_room_center = new Vector3(0,0,0);
        //Vector3 test_room_dimension = new Vector3(10, 10, 10);
        //float refl_percent = 95.0f;
        //addRoom(new RS3DGameDataRoom(0, 3, 0, 0, test_room_center, test_room_dimension, refl_percent, refl_percent, refl_percent, refl_percent, refl_percent, refl_percent));
        //sendData();

        setRoomsToFindableRS3DRooms();
        setOccludersToFindableRS3DOccluders();
        sendData();
    }

    // Update is called once per frame
    void Update ()
    {
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public struct RS3DGameDataStateHeader
    {
        public int version_number;
        public int tick;
        public int num_rooms;
        public int num_occluders;

        public int computeExpectedBlobSize()
        {
            return System.Runtime.InteropServices.Marshal.SizeOf(typeof(RS3DGameDataStateHeader)) + System.Runtime.InteropServices.Marshal.SizeOf(typeof(RS3DGameDataRoom)) * num_rooms + System.Runtime.InteropServices.Marshal.SizeOf(typeof(RS3DGameDataSingleTriangleOccluder)) * num_occluders;
        }
    };


    public struct RS3DGameDataRoom
    {
        public int room_ID;

        public int order_early_reflection;
        public int order_late_reflection;
        public float reverb_length_ms;

        public Vector3 center;
        public Vector3 dimensions;

        //Reflection percentages
        public float refl_percent_left;
        public float refl_percent_right;
        public float refl_percent_front;
        public float refl_percent_back;
        public float refl_percent_floor;
        public float refl_percent_ceil;

        // add for all
        public float all_refl;

        public RS3DGameDataRoom(int _room_ID, int _order_early_reflection, int _order_late_reflection, float _reverb_length_ms,
                         Vector3 _center, Vector3 _dimensions,
                                float _refl_percent_left,
                                float _refl_percent_right,
                                float _refl_percent_front,
                                float _refl_percent_back,
                                float _refl_percent_floor,
                                float _refl_percent_ceil,
                                float _all_refl)
        {
            room_ID = _room_ID;
            order_early_reflection = _order_early_reflection;
            order_late_reflection = _order_late_reflection;
            reverb_length_ms = _reverb_length_ms;
            center = _center;
            dimensions = _dimensions;
            all_refl = _all_refl;

            if (all_refl == -1)
            {
                refl_percent_left = _refl_percent_left;
                refl_percent_right = _refl_percent_right;
                refl_percent_front = _refl_percent_front;
                refl_percent_back = _refl_percent_back;
                refl_percent_floor = _refl_percent_floor;
                refl_percent_ceil = _refl_percent_ceil;
            }
            else
            {
                refl_percent_left = all_refl;
                refl_percent_right = all_refl;
                refl_percent_front = all_refl;
                refl_percent_back = all_refl;
                refl_percent_floor = all_refl;
                refl_percent_ceil = all_refl;
            }



        }
    };

    public struct RS3DGameDataSingleTriangleOccluder
    {
        public int occluder_ID;

        public Vector3 vertex_A;
        public Vector3 vertex_B;
        public Vector3 vertex_C;

        public float absorption_percent;

        public RS3DGameDataSingleTriangleOccluder(int _occluder_ID, 
            Vector3 _vertex_A, Vector3 _vertex_B, Vector3 _vertex_C,
            float _absorption_percent)
        {
            occluder_ID = _occluder_ID;

            vertex_A = _vertex_A;
            vertex_B = _vertex_B;
            vertex_C = _vertex_C;

            absorption_percent = _absorption_percent;
        }
    };

    RS3DGameDataStateHeader RS3D_game_data_state_header;
    List<RS3DGameDataRoom> RS3D_game_data_rooms = new List<RS3DGameDataRoom>();
    List<RS3DGameDataSingleTriangleOccluder> RS3D_game_data_single_triangle_occluders = new List<RS3DGameDataSingleTriangleOccluder>();

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void addRoom(RS3DGameDataRoom  _RS3D_game_data_room) {
	    RS3D_game_data_rooms.Add(_RS3D_game_data_room);
	    RS3D_game_data_state_header.num_rooms = RS3D_game_data_rooms.Count;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void addSingleTriangleOccluder(RS3DGameDataSingleTriangleOccluder _RS3D_game_data_single_triangle_occluder)
    {
        RS3D_game_data_single_triangle_occluders.Add(_RS3D_game_data_single_triangle_occluder);
        RS3D_game_data_state_header.num_occluders = RS3D_game_data_single_triangle_occluders.Count;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void updateRoom(RS3DGameDataRoom _RS3D_game_data_room, int idx)
    {
        if(idx < RS3D_game_data_rooms.Count)
        {
            RS3D_game_data_rooms[idx] = _RS3D_game_data_room;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void updateSingleTriangleOccluder(RS3DGameDataSingleTriangleOccluder _RS3D_game_data_single_triangle_occluder, int idx)
    {
        if (idx < RS3D_game_data_single_triangle_occluders.Count)
        {
            RS3D_game_data_single_triangle_occluders[idx] = _RS3D_game_data_single_triangle_occluder;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void setRoomsToFindableRS3DRooms()
    {
        RS3D_game_data_rooms.Clear();

        //Retrieve all RS3DRoom objects
        RS3DRoom[] RS3D_room_list = FindObjectsOfType(typeof(RS3DRoom)) as RS3DRoom[];
        foreach (RS3DRoom room in RS3D_room_list) {
            addRoom(new RS3DGameDataRoom(room.GetInstanceID(), room.order_early_reflection, room.order_late_reflection, room.reverb_length_ms,
                    room.transform.position, room.transform.localScale, 
                    room.refl_percent_left, room.refl_percent_right, room.refl_percent_front, room.refl_percent_back, room.refl_percent_floor, room.refl_percent_ceil, room.all_refl));
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void setOccludersToFindableRS3DOccluders()
    {
        RS3D_game_data_single_triangle_occluders.Clear();

        //Retrieve all RS3DOccluder objects
        RS3DOccluder[] RS3D_occluder_list = FindObjectsOfType(typeof(RS3DOccluder)) as RS3DOccluder[];
        foreach (RS3DOccluder occluder in RS3D_occluder_list)
        {
            List<Vector3> triangle_vertex_list = occluder.GetTriangleVertexList();
            int num_triangles = triangle_vertex_list.Count / 3;
          //  Debug.Log("num_triangles " + num_triangles);
            for (int i = 0; i < num_triangles; ++i) {
                addSingleTriangleOccluder(new RS3DGameDataSingleTriangleOccluder(occluder.GetInstanceID(), 
                    triangle_vertex_list[i * 3 + 0], triangle_vertex_list[i * 3 + 1], triangle_vertex_list[i * 3 + 2], 
                    occluder.absorption_percent));

            //    Debug.Log("Triangle " + i + ": " + triangle_vertex_list[i * 3 + 0] + ", " + triangle_vertex_list[i * 3 + 1] + ", " + triangle_vertex_list[i * 3 + 2] );

            }
        }


    }

    //////////////////////Marshall data and send to Wwise plugin
    public void sendData()
    {

        int blob_size = RS3D_game_data_state_header.computeExpectedBlobSize();
        Debug.Log("Blob size: " + blob_size);

        byte[] buffer = new byte[blob_size];

        //Increment header tick
        RS3D_game_data_state_header.tick++;	//Increment tick

        //Write header data to buffer
        int start_idx = 0;
        int header_byte_size = Marshal.SizeOf(typeof(RS3DGameDataStateHeader));
        IntPtr ptr = Marshal.AllocHGlobal(header_byte_size);
        Marshal.StructureToPtr(RS3D_game_data_state_header, ptr, true);
        Marshal.Copy(ptr, buffer, start_idx, header_byte_size);
        start_idx = start_idx + header_byte_size;
        Marshal.FreeHGlobal(ptr);

        //Write room data to buffer
        int room_byte_size = Marshal.SizeOf(typeof(RS3DGameDataRoom));
        ptr = Marshal.AllocHGlobal(room_byte_size);
        foreach (RS3DGameDataRoom room in RS3D_game_data_rooms)
        {
            Marshal.StructureToPtr(room, ptr, true);
            Marshal.Copy(ptr, buffer, start_idx, room_byte_size);
            start_idx = start_idx + room_byte_size;
        }
        Marshal.FreeHGlobal(ptr);


        //Write occluder data to buffer
        int occluder_byte_size = Marshal.SizeOf(typeof(RS3DGameDataSingleTriangleOccluder));
        ptr = Marshal.AllocHGlobal(occluder_byte_size);
        foreach (RS3DGameDataSingleTriangleOccluder occluder in RS3D_game_data_single_triangle_occluders)
        {
            Marshal.StructureToPtr(occluder, ptr, true);
            Marshal.Copy(ptr, buffer, start_idx, occluder_byte_size);
            start_idx = start_idx + occluder_byte_size;
        }
        Marshal.FreeHGlobal(ptr);

        //Write managed byte array to unamanged intptr
        ptr = Marshal.AllocHGlobal(blob_size);
        Marshal.Copy(buffer, 0, ptr, blob_size);

        uint RS3D_mixer_bus_ID = AkSoundEngine.GetIDFromString("RealSpace3D Bus");
        AKRESULT res = AkSoundEngine.SendPluginCustomGameData(RS3D_mixer_bus_ID, AkSoundEngine.AK_MIXER_FX_SLOT, ptr, (uint)blob_size);

        if (res == AKRESULT.AK_Success)
        {
            Debug.Log("SendPluginCustomGameData SUCCESS\n");
        }
        else
        {
            Debug.Log("SendPluginCustomGameData FAIL, check Mixer bus ID\n");
        }
        Marshal.FreeHGlobal(ptr);

    }

    // update rooms
    public void UpdateSize()
    {
        setRoomsToFindableRS3DRooms();
        setOccludersToFindableRS3DOccluders();
        sendData();
    }

}

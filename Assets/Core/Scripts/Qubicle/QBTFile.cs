using System.IO;
using UnityEngine;
using Ionic.Zlib;
using System.Collections.Generic;

public class QBTFile {

	VoxelData[,,] voxelsData;
	public VoxelData[,,] VoxelsData { get { return voxelsData; } }

	HashSet<Color> colors;
	public HashSet<Color> Colors { get { return colors; } }

	bool mergeCompounds = false;

	public QBTFile(string path) {
		colors = new HashSet<Color>();

		using (var readStream = File.OpenRead(path)) {
			LoadQB2(new BinaryReader(readStream));
		}
	}

	public void LoadQB2(BinaryReader stream) {
		// Load Header
		int magic = stream.ReadInt32();
		byte major = stream.ReadByte();
		byte minor = stream.ReadByte();

		Debug.LogFormat("[QBT] version {0}.{1}", major, minor);

		if (magic != 0x32204251) {
			throw new UnityException("magic does not match. Expected: " + 0x32204251 + ", got: " + magic);
		}

		Vector3 globalScale = new Vector3();
		globalScale.x = stream.ReadSingle();
		globalScale.y = stream.ReadSingle();
		globalScale.z = stream.ReadSingle();

		// Load Color Map
		string colormapStr = new string(stream.ReadChars(8)); // = COLORMAP

		if (colormapStr != "COLORMAP") {
			throw new UnityException("expected 'COLORMAP', got " + colormapStr);
		}

		uint colorCount = stream.ReadUInt32();

		Debug.LogFormat("[QBT] color count: {0}", colorCount);

		int[] colors = new int[colorCount];
		for (uint i = 0; i < colorCount; i++) {
			colors[i] = stream.ReadInt32();
		}

		// Load Data Tree
		string datatreeStr = new string(stream.ReadChars(8)); // = DATATREE

		if (datatreeStr != "DATATREE") {
			throw new UnityException("expected 'DATATREE', got " + datatreeStr);
		}

		LoadNode(stream);
	}

	void LoadNode(BinaryReader stream) {
		uint nodeTypeID = stream.ReadUInt32();
		uint dataSize = stream.ReadUInt32();

		switch (nodeTypeID) {
		case 0:
			LoadMatrix(stream);
			break;
		case 1:
			LoadModel(stream);
			break;
		case 2:
			LoadCompound(stream);
			break;
		default:
			stream.ReadBytes((int)dataSize); // skip node if unknown
			break;
		}
	}

	void LoadModel(BinaryReader stream) {
		Debug.Log("[QBT] LoadModel");

		uint childCount = stream.ReadUInt32();
		for (uint i = 0; i < childCount; i++) {
			LoadNode(stream);
		}
	}

	void LoadMatrix(BinaryReader stream) {
		Debug.Log("[QBT] LoadMatrix");

		int nameLength = stream.ReadInt32();
		string name = new string(stream.ReadChars(nameLength));

		Debug.Log("[QBT] name: " + name);

		Vector3 position = new Vector3();
		position.x = stream.ReadInt32();
		position.y = stream.ReadInt32();
		position.z = stream.ReadInt32();

		Vector3 localScale = new Vector3();
		localScale.x = stream.ReadUInt32();
		localScale.y = stream.ReadUInt32();
		localScale.z = stream.ReadUInt32();

		Vector3 pivot = new Vector3();
		pivot.x = stream.ReadSingle();
		pivot.y = stream.ReadSingle();
		pivot.z = stream.ReadSingle();

		Vector3 size = new Vector3();
		size.x = stream.ReadUInt32();
		size.y = stream.ReadUInt32();
		size.z = stream.ReadUInt32();

		// Compressed voxel data size.
		stream.ReadUInt32();

		voxelsData = new VoxelData[(int)size.x, (int)size.y, (int)size.z];

		var zlibStream = new ZlibStream(stream.BaseStream, Ionic.Zlib.CompressionMode.Decompress, true);
		for (uint x = 0; x < size.x; x++) {
			for (uint z = 0; z < size.z; z++) {
				for (uint y = 0; y < size.y; y++) {
					VoxelData data = new VoxelData();

					data.r = zlibStream.ReadByte();
					data.g = zlibStream.ReadByte();
					data.b = zlibStream.ReadByte();
					data.m = zlibStream.ReadByte();

					voxelsData[x, y, z] = data;
					colors.Add(data.Color);
				}
			}
		}
	}

	void LoadCompound(BinaryReader stream) {
		Debug.Log("[QBT] LoadCompound");

		int nameLength = stream.ReadInt32();
		string name = new string(stream.ReadChars(nameLength));

		Debug.Log("[QBT] name: " + name);

		Vector3 position = new Vector3();
		position.x = stream.ReadInt32();
		position.y = stream.ReadInt32();
		position.z = stream.ReadInt32();

		Vector3 localScale = new Vector3();
		localScale.x = stream.ReadUInt32();
		localScale.y = stream.ReadUInt32();
		localScale.z = stream.ReadUInt32();

		Vector3 pivot = new Vector3();
		pivot.x = stream.ReadSingle();
		pivot.y = stream.ReadSingle();
		pivot.z = stream.ReadSingle();

		Vector3 size = new Vector3();
		size.x = stream.ReadUInt32();
		size.y = stream.ReadUInt32();
		size.z = stream.ReadUInt32();

		// Compressed voxel data size.
		stream.ReadUInt32();

		// TODO: voxelsData is overwrited
		voxelsData = new VoxelData[(int)size.x, (int)size.y, (int)size.z];

		var zlibStream = new ZlibStream(stream.BaseStream, Ionic.Zlib.CompressionMode.Decompress, true);
		for (uint x = 0; x < size.x; x++) {
			for (uint z = 0; z < size.z; z++) {
				for (uint y = 0; y < size.y; y++) {
					VoxelData data = new VoxelData();

					data.r = zlibStream.ReadByte();
					data.g = zlibStream.ReadByte();
					data.b = zlibStream.ReadByte();
					data.m = zlibStream.ReadByte();

					voxelsData[x, y, z] = data;
				}
			}
		}

		uint childCount = stream.ReadUInt32();
		if (mergeCompounds) { // if you don't need the datatree you can skip child nodes
			for (uint i = 0; i < childCount; i++) {
				SkipNode(stream);
			}
		} else {
			for (uint i = 0; i < childCount; i++) {
				LoadNode(stream);
			}
		}
	}

	void SkipNode(BinaryReader stream) {
		Debug.Log("[QBT] SkipNode");

		stream.ReadInt32(); // node type, can be ignored
		uint dataSize = stream.ReadUInt32();
		stream.ReadChars((int) dataSize);
	}

	public struct VoxelData {
		public int r;
		public int g;
		public int b;
		public int m;

		public override string ToString() {
			return r + ", " + g + ", " + b + ", " + m;
		}

		public Color Color { get { return new Color(1f / r, 1f / g, 1f / b, 1); } }
	}

}

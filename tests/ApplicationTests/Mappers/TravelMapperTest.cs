using Application.DTOs;
using Application.Mappers;

namespace ApplicationTests.Mappers;

public class TravelMapperTest
{
    [Fact]
    public void ToExcelDocument_EmptyList_ReturnOneDataSheet()
    {
        var data = new List<TravelDTO>();

        var result = TravelMapper.ToExcelDocument(data);

        Assert.NotNull(result);
        Assert.Equal(1,result?.Sheets.Count);
        Assert.NotEqual(0, result?.Sheets[0].Columns.Count);
        Assert.Equal(0, result?.Sheets[0].Rows.Count);
    }

    [Fact]
    public void ToExcelDocument_OneVehiclePlateInList_ReturnOneDataSheet()
    {

        var data = new List<TravelDTO>()
        {
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                TravelDestination = "destino1",
                Created = DateTime.UtcNow,
            },
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                TravelDestination = "destino2",
                Created = DateTime.UtcNow,
            },
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                TravelDestination = "destino3",
                Created = DateTime.UtcNow,
            }
        };

        var result = TravelMapper.ToExcelDocument(data);

        Assert.NotNull(result);
        Assert.Equal(1, result?.Sheets.Count);
        Assert.NotEqual(0, result?.Sheets[0].Columns.Count);
        Assert.Equal(3, result?.Sheets[0].Rows.Count);
    }

    [Fact]
    public void ToExcelDocument_TwoVehiclePlateInList_ReturnTwoDataSheet()
    {

        var data = new List<TravelDTO>()
        {
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                TravelDestination = "destino1",
                Created = DateTime.UtcNow,
            },
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A124",
                TravelDestination = "destino2",
                Created = DateTime.UtcNow,
            },
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                TravelDestination = "destino3",
                Created = DateTime.UtcNow,
            },
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                TravelDestination = "destino3",
                Created = DateTime.UtcNow,
            }
        };

        var result = TravelMapper.ToExcelDocument(data);

        Assert.NotNull(result);
        Assert.Equal(2, result?.Sheets.Count);
        Assert.NotEqual(0, result?.Sheets[0].Columns.Count);
        Assert.Equal(3, result?.Sheets[0].Rows.Count);
        Assert.Equal(1, result?.Sheets[1].Rows.Count);
    }

    [Fact]
    public void ToExcelDocument_validListTravelDTO_ReturnSameNumberColumnsHeaderAndColumn()
    {

        var data = new List<TravelDTO>()
        {
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                TravelDestination = "destino1",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(1),
                Created = DateTime.UtcNow,
            },
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                Start = DateTime.UtcNow,
                TravelDestination = "destino2",
                Created = DateTime.UtcNow,
            },
            new TravelDTO()
            {
                Id = Guid.NewGuid(),
                VehiclePlate = "ASD1A123",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(1),
                Autonomy = 2,
                TravelDestination = "destino3",
                Created = DateTime.UtcNow,
            }
        };

        var result = TravelMapper.ToExcelDocument(data);
        var qtdHeaderCols = result?.Sheets[0].Columns.Count;

        Assert.NotNull(result);
        Assert.Equal(1, result?.Sheets.Count);
        Assert.NotEqual(0, result?.Sheets[0].Columns.Count);
        Assert.Equal(qtdHeaderCols, result?.Sheets[0].Rows[0].Values.Count());
        Assert.Equal(qtdHeaderCols, result?.Sheets[0].Rows[1].Values.Count());
        Assert.Equal(qtdHeaderCols, result?.Sheets[0].Rows[2].Values.Count());
    }
}

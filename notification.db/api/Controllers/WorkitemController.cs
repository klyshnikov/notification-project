using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using models.entity;
using MongoDB.Bson;
using MongoDB.Driver;
using notification.db.Requests;
using repo;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text.Json;

namespace notification.db.Controllers;

public class WorkitemController : Controller
{
    private readonly IMongoCollection<BsonDocument> _collection;
    private readonly AppDbContext dbContext;

    public WorkitemController(IMongoDatabase database, AppDbContext dbContext)
    {
        _collection = database.GetCollection<BsonDocument>("workitems");
        this.dbContext = dbContext;
    }

    [HttpPut("/create-wi")]
    public IActionResult CreateWi([FromBody] CreateWiRequest request)
    {
        if (request == null)
        {
            return BadRequest("Данные не были переданы.");
        }

        Dictionary<string, JsonElement> document = new Dictionary<string, JsonElement> ()
        {
            { "WiId", JsonSerializer.SerializeToElement(GetNextId()) },
            { "Title", JsonSerializer.SerializeToElement(request.Title) },
            { "Description", JsonSerializer.SerializeToElement(request.Description) },
            { "StartTime", JsonSerializer.SerializeToElement(request.StartTime) },
            { "EndTime", JsonSerializer.SerializeToElement(request.EndTime) },
            { "AuthorId", JsonSerializer.SerializeToElement(request.AuthorId) },
            { "CreatedTime", JsonSerializer.SerializeToElement(request.CreatedTime) },
            { "TeamId", JsonSerializer.SerializeToElement(dbContext.Teams.FirstOrDefault(t => t.ChatId == request.ChatId).Id)},
            { "Username", JsonSerializer.SerializeToElement(dbContext.Users.FirstOrDefault(u => request.AuthorId == u.Id).Username)}
        };

        var bsonDocument = ConvertJsonElementToBsonDocument(document);

        _collection.InsertOne(bsonDocument);

        var responseDocument = ConvertBsonDocumentToDictionary(bsonDocument);

        return Ok();
    }
    
    [HttpGet("/get-wi")]
    public IActionResult GetWi([FromQuery] string wiId)
    {
        if (string.IsNullOrEmpty(wiId))
        {
            return BadRequest("Параметр wiId обязателен.");
        }

        if (!int.TryParse(wiId, out int wiIdInt))
        {
            return BadRequest("Параметр wiId должен быть числом.");
        }

        var filter = Builders<BsonDocument>.Filter.Eq("WiId", wiIdInt);

        var documents = _collection.Find(filter).ToList();

        var result = new List<Dictionary<string, object>>();
        foreach (var doc in documents)
        {
            result.Add(doc.ToDictionary());
        }

        return Ok(result.FirstOrDefault());
    }
    

    [HttpGet("/get-all-wi-in-chat")]
    public IActionResult GetAllWi([FromQuery] string chatId)
    {
        string teamId = dbContext.Teams.FirstOrDefault(t => t.ChatId == chatId).Id;

        var filter = Builders<BsonDocument>.Filter.Eq("TeamId", teamId);

        var documents = _collection.Find(filter).ToList();

        var result = new List<Dictionary<string, object>>();
        foreach (var doc in documents)
        {
            result.Add(doc.ToDictionary());
        }

        return Ok(result);
    }

    [HttpPost("/change-wi")]
    public IActionResult ChangeWi([FromQuery] string wiId, [FromQuery] string fieldName, [FromQuery] string fieldValue)
    {
        if (string.IsNullOrEmpty(wiId) || string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
        {
            return BadRequest("Все параметры (wiId, fieldName, fieldValue) обязательны.");
        }

        if (!int.TryParse(wiId, out int wiIdInt))
        {
            return BadRequest("Параметр wiId должен быть числом.");
        }

        var filter = Builders<BsonDocument>.Filter.Eq("WiId", wiIdInt);

        var update = Builders<BsonDocument>.Update.Set(fieldName, fieldValue);

        var updateResult = _collection.UpdateOne(filter, update);

        if (updateResult.MatchedCount > 0)
        {
            return Ok(new { message = $"Поле '{fieldName}' у документа с WiId = {wiIdInt} успешно обновлено." });
        }
        else
        {
            return NotFound(new { message = $"Документ с WiId = {wiIdInt} не найден." });
        }
    }

    [HttpDelete("/delete-wi")]
    public IActionResult DeleteWi([FromQuery] string wiId)
    {
        if (string.IsNullOrEmpty(wiId))
        {
            return BadRequest("Параметр wiId обязателен.");
        }

        if (!int.TryParse(wiId, out int wiIdInt))
        {
            return BadRequest("Параметр wiId должен быть числом.");
        }

        var filter = Builders<BsonDocument>.Filter.Eq("WiId", wiIdInt);

        var deleteResult = _collection.DeleteOne(filter);

        if (deleteResult.DeletedCount > 0)
        {
            return Ok(new { message = $"Документ с WiId = {wiIdInt} успешно удален." });
        }
        else
        {
            return NotFound(new { message = $"Документ с WiId = {wiIdInt} не найден." });
        }
    }

    [HttpPut("/assign-to")]
    public IActionResult AssignTo([FromQuery] string wiId, [FromQuery] string username)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(wiId))
        {
            return BadRequest("Параметры обязателены.");
        }

        string userId = dbContext.Users.FirstOrDefault(u => u.Username == username).Id;

        var filter = Builders<BsonDocument>.Filter.Eq("WiId", int.Parse(wiId));
        var update = Builders<BsonDocument>.Update.Set("AssignTo", userId);

        var result = _collection.UpdateOne(filter, update);

        if (result.MatchedCount == 0)
        {
            return NotFound($"Документ с WiId = {wiId} не найден");
        }

        return Ok();
    }

    [HttpGet("/get-my-wi")]
    public IActionResult GetMyWi([FromQuery] string userId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("AssignTo", userId);

        var documents = _collection.Find(filter).ToList();

        var result = new List<Dictionary<string, object>>();
        foreach (var doc in documents)
        {
            result.Add(doc.ToDictionary());
        }

        return Ok(result);
    }

    [HttpPut("/close-wi")]
    public IActionResult CloseWi([FromQuery] string wiId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("WiId", int.Parse(wiId));

        var update = Builders<BsonDocument>.Update.Unset("AssignTo");

        var result = _collection.UpdateOne(filter, update);

        if (result.MatchedCount == 0)
        {
            return NotFound($"Документ с WiId = {wiId} не найден");
        }

        return Ok();
    }

    private int GetNextId()
    {
        var pipeline = new[]
        {
            new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "maxWiId", new BsonDocument("$max", "$WiId") }
            })
        };

        var result = _collection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();
        try
        {
            var maxWiId = result?["maxWiId"].AsInt32;
            return maxWiId + 1 ?? 1;
        }
        catch (Exception ex)
        {
            return 1;
        }
    }

    private BsonDocument ConvertJsonElementToBsonDocument(Dictionary<string, JsonElement> dictionary)
    {
        var bsonDocument = new BsonDocument();

        foreach (var keyValuePair in dictionary)
        {
            bsonDocument.Add(keyValuePair.Key, ConvertJsonElementToBsonValue(keyValuePair.Value));
        }

        return bsonDocument;
    }

    private BsonValue ConvertJsonElementToBsonValue(JsonElement jsonElement)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Undefined:
            case JsonValueKind.Null:
                return BsonNull.Value;

            case JsonValueKind.True:
            case JsonValueKind.False:
                return jsonElement.GetBoolean();

            case JsonValueKind.Number:
                if (jsonElement.TryGetInt32(out int intValue))
                {
                    return intValue;
                }
                if (jsonElement.TryGetInt64(out long longValue))
                {
                    return longValue;
                }
                return jsonElement.GetDouble();

            case JsonValueKind.String:
                return jsonElement.GetString();

            case JsonValueKind.Object:
                var nestedDictionary = new Dictionary<string, JsonElement>();
                foreach (var property in jsonElement.EnumerateObject())
                {
                    nestedDictionary.Add(property.Name, property.Value);
                }
                return ConvertJsonElementToBsonDocument(nestedDictionary);

            case JsonValueKind.Array:
                var bsonArray = new BsonArray();
                foreach (var item in jsonElement.EnumerateArray())
                {
                    bsonArray.Add(ConvertJsonElementToBsonValue(item));
                }
                return bsonArray;
            default:
                throw new ArgumentException($"Unsupported JsonValueKind: {jsonElement.ValueKind}");
        }
    }

    private Dictionary<string, object> ConvertBsonDocumentToDictionary(BsonDocument bsonDocument)
    {
        var dictionary = new Dictionary<string, object>();

        foreach (var element in bsonDocument.Elements)
        {
            dictionary.Add(element.Name, ConvertBsonValueToObject(element.Value));
        }

        return dictionary;
    }

    private object ConvertBsonValueToObject(BsonValue bsonValue)
    {
        switch (bsonValue.BsonType)
        {
            case BsonType.Null:
                return null;

            case BsonType.Boolean:
                return bsonValue.AsBoolean;

            case BsonType.Int32:
                return bsonValue.AsInt32;

            case BsonType.Int64:
                return bsonValue.AsInt64;

            case BsonType.Double:
                return bsonValue.AsDouble;

            case BsonType.String:
                return bsonValue.AsString;

            case BsonType.ObjectId:
                return bsonValue.AsObjectId.ToString();

            case BsonType.Document:
                return ConvertBsonDocumentToDictionary(bsonValue.AsBsonDocument);

            case BsonType.Array:
                var list = new List<object>();
                foreach (var item in bsonValue.AsBsonArray)
                {
                    list.Add(ConvertBsonValueToObject(item));
                }
                return list;

            default:
                throw new ArgumentException($"Unsupported BsonType: {bsonValue.BsonType}");
        }
    }
}

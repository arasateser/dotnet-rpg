global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character(),
            new Character{Id =1, Name = "Sam"}
    };
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);

            // characters.Add(newCharacter);
            // characters.Add(_mapper.Map<Character>(newCharacter));

            // serviceResponse.Data = characters;
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            return serviceResponse;

            // characters.Add(newCharacter);
            // return characters;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacters(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

            try
            {
                var character = characters.First(c => c.Id == id);
                if (character == null)
                    throw new Exception($"Character with Id {id} not found.");

                characters.Remove(character);
                serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharactters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Characters.ToListAsync();

            // serviceResponse.Data = characters;
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            return serviceResponse;
            // return characters;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var dbCharacters = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            // var character = characters.FirstOrDefault(c => c.Id == id);
            // serviceResponse.Data = character;
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacters);
            return serviceResponse;

            /*if (character is not null)
                return character;

            throw new Exception("Character Not Found");*/

            //return characters.FirstOrDefault(character => character.Id == id);
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
                if (character == null)
                { throw new Exception($"Character with Id '{updatedCharacter.Id}' not found"); }

                // _mapper.Map<Character>(updatedCharacter); updating also possible using automapper
                // _mapper.Map(UpdateCharacter, character);

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Class = updatedCharacter.Class;
                character.Intelligence = updatedCharacter.Intelligence;

                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = true;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
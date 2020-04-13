using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;

namespace Jineo.Helpers
{
    public class MapperHendler
    {   
        public IMapper mapper;

        public MapperHendler(IMapper _mapper)
        {
            mapper = _mapper;
        }

        public T2 ExecuteMap<T1, T2>(T1 from)
        {
            return mapper.Map<T2>(from);
        }

        public IEnumerable ExecuteMap<T1,T2>(IEnumerable<T1> from)
        {
            return mapper.Map<List<T2>>(from);
        }
    }
}
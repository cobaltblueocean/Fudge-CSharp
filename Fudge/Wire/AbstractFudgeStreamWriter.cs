using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fudge.Taxon ;

namespace Fudge.Wire
{
    public abstract class AbstractFudgeStreamWriter : IFudgeStreamWriter
    {
        private FudgeContext _fudgeContext;
        private IFudgeTaxonomy _taxonomy;
        private int _taxonomyId;

        protected AbstractFudgeStreamWriter(FudgeContext fudgeContext)
        {
            if (fudgeContext == null)
            {
                throw new NullReferenceException("FudgeContext must not be null");
            }
            _fudgeContext = fudgeContext;
        }

        public void StartMessage()
        {
            throw new NotImplementedException();
        }

        public void StartSubMessage(string name, int? ordinal)
        {
            throw new NotImplementedException();
        }

        public void WriteField(string name, int? ordinal, FudgeFieldType type, object value)
        {
        }

        public void WriteFields(IEnumerable<IFudgeField> fields)
        {
            writeAllFields(fields);
        }

        protected void writeAllFields(IEnumerable<IFudgeField> fields)
        {
            foreach (IFudgeField field in fields)
            {
                WriteField(field);
            }
        }

        public void EndSubMessage()
        {
            throw new NotImplementedException();
        }

        public void EndMessage()
        {
            throw new NotImplementedException();
        }

        public FudgeContext FudgeContext
        {
            get { return _fudgeContext; }
        }

        public Taxon.IFudgeTaxonomy CurrentTaxonomy
        {
            get { return _taxonomy; }
        }

        public int CurrentTaxonomyId
        {
            get
            {
                return _taxonomyId;
            }
            set
            {
                _taxonomyId = value;
                _taxonomy = FudgeContext.TaxonomyResolver.ResolveTaxonomy((short)value);
            }
        }

        public void WriteEnvelopeHeader(int processingDirectives, int schemaVersion, int messageSize)
        {
            throw new NotImplementedException();
        }

        public void EnvelopeComplete()
        {
            throw new NotImplementedException();
        }

        public void WriteField(IFudgeField field)
        {
            if (field == null)
            {
                throw new NullReferenceException("FudgeField must not be null");
            }
            WriteField(field.Name, field.Ordinal, field.Type, field.Value);
        }

        public void WriteField(short ordinal, string name, FudgeFieldType type, object fieldValue)
        {
            throw new NotImplementedException();
        }

        public void WriteFields(IFudgeFieldContainer fields)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}

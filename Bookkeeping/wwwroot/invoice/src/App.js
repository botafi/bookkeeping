import React, { Component } from 'react';
import './App.css';

class App extends Component {
  constructor(props, context) {
    super(props, context);
    const { invoiceItems } = window;
    this.state = {
      invoiceItems,
      availableIds: []
    };
  }
  addNewItem = () => {
    const { invoiceItems, availableIds } = this.state;
    this.setState({
      invoiceItems: [
        ...invoiceItems,
        availableIds.length
          ? {
              Amount: null,
              Title: '',
              Price: null,
              Unit: '',
              InvoiceItemId: availableIds.pop()
            }
          : {
              Amount: null,
              Title: '',
              Price: null,
              Unit: ''
            }
      ],
      availableIds: [...availableIds]
    });
  };
  deleteItem = i => {
    const { invoiceItems, availableIds } = this.state;
    const [{ InvoiceItemId }] = invoiceItems.splice(i, 1);
    this.setState({
      invoiceItems: [...invoiceItems]
    });
    if (InvoiceItemId || InvoiceItemId === 0) {
      this.setState({ availableIds: [...availableIds, InvoiceItemId] });
    }
  };
  onItemChanged = (i, key, val) => {
    const { invoiceItems } = this.state;
    invoiceItems[i][key] = val;
    this.setState({ invoiceItems: [...invoiceItems] });
  };
  render() {
    return (
      <div style={{ width: '100%' }}>
        {this.state.availableIds.map((id, i) => (
          <input
            defaultValue={id}
            value={id}
            name={'DeleteInvoiceItems[' + i + ']'}
            type="hidden"
          />
        ))}
        <table className="table" style={{ width: '100%' }}>
          <thead>
            <tr>
              <th style={{ display: 'none' }} />
              <th scope="col" style={{ width: '10%' }}>
                Počet
              </th>
              <th scope="col" style={{ width: '10%' }}>
                Měrná jednotka
              </th>
              <th scope="col">Popis</th>
              <th scope="col" style={{ width: '15%' }}>
                Cena za MJ
              </th>
              <th style={{ width: '5%' }} />
            </tr>
          </thead>
          <tbody>
            {this.state.invoiceItems.map(
              ({ Amount, Title, Price, Unit, InvoiceItemId }, i) => (
                <tr key={i}>
                  <td style={{ display: 'none' }}>
                    {(InvoiceItemId || InvoiceItemId === 0) && (
                      <input
                        defaultValue={InvoiceItemId}
                        value={InvoiceItemId}
                        name={'InvoiceItems[' + i + '].InvoiceItemId'}
                        type="hidden"
                      />
                    )}
                  </td>

                  <td>
                    <input
                      defaultValue={Amount}
                      name={'InvoiceItems[' + i + '].Amount'}
                      id={'InvoiceItems' + i + 'Amount'}
                      type="number"
                      className="form-control"
                      required
                      data-val="true"
                      data-val-required=""
                      onChange={e =>
                        this.onItemChanged(i, 'Amount', e.target.value)
                      }
                    />
                    <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for={'InvoiceItems' + i + 'Amount'}
                      data-valmsg-replace="true"
                    />
                  </td>
                  <td>
                    <input
                      defaultValue={Unit}
                      name={'InvoiceItems[' + i + '].Unit'}
                      id={'InvoiceItems' + i + 'Unit'}
                      type="text"
                      className="form-control"
                      required
                      data-val="true"
                      data-val-required=""
                      onChange={e =>
                        this.onItemChanged(i, 'Unit', e.target.value)
                      }
                    />
                    <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for={'InvoiceItems' + i + 'Unit'}
                      data-valmsg-replace="true"
                    />
                  </td>
                  <td>
                    <input
                      defaultValue={Title}
                      name={'InvoiceItems[' + i + '].Title'}
                      id={'InvoiceItems' + i + 'Title'}
                      type="text"
                      className="form-control"
                      required
                      data-val="true"
                      data-val-required=""
                      onChange={e =>
                        this.onItemChanged(i, 'Title', e.target.value)
                      }
                    />
                    <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for={'InvoiceItems' + i + 'Title'}
                      data-valmsg-replace="true"
                    />
                  </td>
                  <td>
                    <input
                      defaultValue={Price}
                      name={'InvoiceItems[' + i + '].Price'}
                      id={'InvoiceItems' + i + 'Price'}
                      type="number"
                      className="form-control"
                      required
                      data-val="true"
                      data-val-required=""
                      onChange={e =>
                        this.onItemChanged(i, 'Price', e.target.value)
                      }
                    />
                    <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for={'InvoiceItems' + i + 'Price'}
                      data-valmsg-replace="true"
                    />
                  </td>
                  <td>
                    <button
                      type="button"
                      className="btn btn-default"
                      onClick={() => this.deleteItem(i)}
                    >
                      <i className="mdi mdi-delete" aria-hidden="true" />
                    </button>
                  </td>
                </tr>
              )
            )}
          </tbody>
        </table>
        <button
          onClick={this.addNewItem}
          type="button"
          className="btn btn-raised btn-sm"
        >
          Přidat položku
        </button>
        <div className="col-md-6 offset-md-6">
          <hr />
          <p
            style={{
              width: '100%',
              textAlign: 'right',
              fontSize: '1.25em'
            }}
          >
            {this.state.invoiceItems
              .map(i => i.Price * i.Amount)
              .reduce((a, b) => a + b, 0)}{' '}
            Kč
          </p>
        </div>
      </div>
    );
  }
}

export default App;

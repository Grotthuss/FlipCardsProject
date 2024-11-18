import React, { useState } from 'react';
import './AddCardSet.css';
import { Errors } from "./errorEnums";
import { EmptyCardSetNameException } from "./EmptyCardSetNameException";

interface AddCardSetProps {
    onAdd: (id: number, setName: string) => void;
}

const AddCardSet: React.FC<AddCardSetProps> = ({ onAdd }) => {
    const [setName, setSetName] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [styleChange, setStyleChange] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        if (!setName.trim()) {
            try {
                throw new EmptyCardSetNameException();
            } catch (e) {
                if (e instanceof EmptyCardSetNameException) {
                    setError(e.message);
                    setStyleChange(true);
                    setTimeout(() => {
                        setStyleChange(false);
                    }, 2000);
                }
            }
            return;
        }

        try {
            const response = await fetch(`https://localhost:44372/api/Home/CreateFullSet`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userId: 1,
                    name: setName.trim(),
                    flipcardsList: []
                }),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                console.error("Error response:", errorMessage);
                throw new Error(Errors.CREATE_SET + ` Server response: ${errorMessage}`);
            }

            const newCardSet = await response.json();

            if (newCardSet && newCardSet.id && newCardSet.name) {
                onAdd(newCardSet.id, newCardSet.name);
                setSetName('');
            } else {
                throw new Error("Unexpected response format from CreateFullSet API.");
            }
        } catch (err) {
            setError(Errors.CREATE_SET + (err as Error).message);
        }
    };

    return (
        <form className="add-card-set-container" onSubmit={handleSubmit}>
            <h2>Add New Card Set</h2>

            <input
                type="text"
                placeholder="Enter card set name"
                value={setName}
                onChange={(e) => setSetName(e.target.value)}
                className={styleChange ? 'input-error' : ''}
            />

            {error && <p className="error-message">{error}</p>}

            <button type="submit">Add Card Set</button>
        </form>
    );
};

export default AddCardSet;